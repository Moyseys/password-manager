using System.Text;
using Core.Contexts;
using Core.Utils;
using DAL;
using DAL.Dtos;
using DAL.Entities;
using DAL.Extensions;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Vaultify.Core.Mappers;
using Vaultify.Features.Secrets.Dtos.Requests;
using Vaultify.Features.Secrets.Dtos.Response;


namespace Vaultify.Features.Secrets;

public class SecretService(
    SecretRepository secretRepository, 
    SecretKeyRepository secretKeyRepository, 
    PasswordManagerDbContext context, 
    UserContext userContext,
    ILogger<SecretService> logger
    )
{
    private readonly SecretRepository _secretRepository = secretRepository;
    private readonly SecretKeyRepository _secretKeyRepository = secretKeyRepository;
    private readonly PasswordManagerDbContext _context = context;
    private readonly UserContext _userContext = userContext;
    private readonly ILogger<SecretService> _logger = logger;

    public async Task CreateSecret(SecretRequestCreateDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var userSecretKey = await GetUserSecretKey(userId);

        var vaultKey = ExtractVaultKey(payload.MasterPassword, userSecretKey.User!.MasterPasswordSalt, userSecretKey.Key);

        Secret secret =  new()
        {
            Title = payload.Title,
            Username = payload.Username,
            Password = CreateSecretPass(vaultKey, Encoding.UTF8.GetBytes(payload.Password)),
            UserId = userId
        };

        await _secretRepository.AddAsync(secret);
    }

    public async Task<PageableDto<SecretResponseListDto>> ListSecrets(PaginationDto pagination)
    {
        var userId = _userContext.GetUserIdOrThrow();
        _logger.LogInformation("[ListSecrets] UserId: {UserId}", userId);
        
        var query = _context.Secret
            .AsNoTracking()
            .Where((s) => s.UserId == userId && s.Active.Equals(true))
            .OrderByDescending(s => s.CreatedAt);
        
        return await query.WithPagination(s => s.ToSecretReponseListDto(), pagination);
    }

    public async Task<SecretResponseDto> GetSecret(Guid secretId, SecretRequestShowDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var userSecretKey = await GetUserSecretKey(userId);
        var secret = await GetUserSecret(userId, secretId);
        
        var vaultKey = ExtractVaultKey(payload.MasterPassword, userSecretKey.User!.MasterPasswordSalt, userSecretKey.Key);
        secret.Password = AESHelper.Decrypt(vaultKey, secret.Password);
        
        return secret.ToSecretResponseDto();
    }

    public async Task<SecretResponseUpdateDto> UpdateSecret(Guid secretId, SecretRequestUpdateDto payload, CancellationToken cancellationToken)
    {
        //TODO Criar validação condicional
        var secret = await GetUserSecret(_userContext.UserId, secretId);
        var secretKey = await GetUserSecretKey(_userContext.UserId);

        secret.ToSecretRequestUpdate(payload);
        if(payload.Password != null && payload.MasterPassword != null)
        {
            var vaultKey = ExtractVaultKey(payload.MasterPassword, secretKey.User!.MasterPasswordSalt, secretKey.Key);
            secret.Password = CreateSecretPass(vaultKey, Encoding.UTF8.GetBytes(payload.Password));
        }

        await _context.SaveChangesAsync(cancellationToken);
        return new SecretResponseUpdateDto(secret.Title, secret.Username, "*****");
    }

    private async Task<SecretKey> GetUserSecretKey(Guid? userId)
    {
        if(userId == null) throw new UnauthorizedAccessException("User is not authenticated!");
        return await _secretKeyRepository.GetSecretKeyByUserId(userId.Value) ?? throw new KeyNotFoundException("User secret key not found");
    }

    private async Task<Secret> GetUserSecret(Guid? userId, Guid secretId)
    {
        if(userId == null) throw new UnauthorizedAccessException("User is not authenticated!");
        return await _secretRepository.GetSecretByIdAndUserId(userId.Value, secretId) ?? throw new KeyNotFoundException("User secret not found");
    }

    private byte[] CreateSecretPass(byte[] userSecretKey, byte[] pass){
        return AESHelper.Encrypt(userSecretKey, pass);
    }

    private byte[] ExtractVaultKey(string masterPass, byte[] masterPassSalt, byte[] userSecretKey ){
        var masterPassDerived = DeriveHelper.RFC2898(masterPass, masterPassSalt) 
            ?? throw new ArgumentException("Secret key invalid");
       return AESHelper.Decrypt(masterPassDerived, userSecretKey);
    }
};
