using System.Linq.Expressions;
using System.Text;
using PasswordManager.DAL;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Secrets.Dtos.Requests;
using PasswordManager.Features.Secrets.Dtos.Response;
using PasswordManager.Shared;
using PasswordManager.SharedDtos;
using PasswordManager.Utils;
using Secret = PasswordManager.DAL.Entities.Secret;


namespace PasswordManager.Features.Secrets;

public class SecretService
{
    private SecretRepository _secretRepository;
    private SecretKeyRepository _secretKeyRepository;
    private PasswordManagerDbContext _context;

    public SecretService(SecretRepository secretRepository,  SecretKeyRepository secretKeyRepository, PasswordManagerDbContext context)
    {
        _secretRepository = secretRepository;
        _secretKeyRepository = secretKeyRepository;
        _context = context;
    }

    public async Task CreateSecret(SecretRequestCreateDto payload, Guid userId)
    {
        var userSecretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId) 
            ?? throw new ArgumentException("Chave secreta inválida");
        var masterPassDerived = DeriveHelper.RFC2898(payload.MasterPassword, userSecretKey.User.MasterPasswordSalt) 
            ?? throw new ArgumentException("Chave secreta inválida 2");
        
        var key = AESHelper.Decrypt(masterPassDerived, userSecretKey.Key);

        Secret secret = new Secret
        {
            Title = payload.Title,
            Username = payload.UserName,
            Password = AESHelper.Encrypt(key, Encoding.UTF8.GetBytes(payload.Password)),
            UserId = userId
        };

        await _secretRepository.AddAsync(secret);
    }

    public async Task<PageableDto<SecretResponseDto>> ListSecrets(Guid userId, PaginationDto pagination)
    {
        Expression<Func<Secret, SecretResponseDto>> projection = s => new SecretResponseDto
        {
            Id = s.Id,
            Title = s.Title,
            UserName = s.Username,
            Password = string.Empty
        };
        return await _context.Secret
            .Where((s) => s.UserId == userId)
            .WithPagination(projection, pagination);
    }

    public async Task<SecretResponseDto> GetSecret(Guid userId, Guid secretId, SecretRequestShowDto payload)
    {
        var userSecretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId) ?? throw new ArgumentException("Chave secreta inválida");

        var secret = await _secretRepository.GetSecretById(secretId) 
            ?? throw new BadHttpRequestException("Not found secret with this id");

        var masterDerived = DeriveHelper.RFC2898(payload.MasterPassword, userSecretKey.User.MasterPasswordSalt);
        var vaultKey = AESHelper.Decrypt(masterDerived, userSecretKey.Key);
        var pass = AESHelper.Decrypt(vaultKey, secret.Password);
        
        return new SecretResponseDto {
            Id = secret.Id,
            Title = secret.Title,
            UserName = secret.Username,
            Password = Encoding.UTF8.GetString(pass)
        };
    }
};
