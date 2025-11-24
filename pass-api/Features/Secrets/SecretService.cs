using System.Linq.Expressions;
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
        var userSecretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId) ?? throw new ArgumentException("Chave secreta inválida");

        Secret secret = new Secret
        {
            Title = payload.Title,
            Username = payload.UserName,
            Password = payload.Password = AESHelper.Encrypt(userSecretKey.Key, payload.Password),
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
            Password = s.Password
        };
        return await _context.Secret.WithPagination(projection, pagination);
    }

    public async Task<SecretResponseDto> GetSecret(Guid userId, Guid secretId)
    {
        var userSecretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId) ?? throw new ArgumentException("Chave secreta inválida");

        Expression<Func<Secret, SecretResponseDto>> projection = s => new SecretResponseDto
        {
            Id = s.Id,
            Title = s.Title,
            UserName = s.Username,
            Password = s.Password 
        };

        var secret = await _secretRepository.GetSecretById<SecretResponseDto>(secretId, projection) 
            ?? throw new BadHttpRequestException("Not found secret with this id");
        
        // Descriptografa DEPOIS de trazer do banco
        secret.Password = AESHelper.Decrypt(userSecretKey.Key, secret.Password);
        
        return secret;
    }
};
