using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Secrets.Dtos.Requests;
using PasswordManager.Features.Secrets.Dtos.Response;
using PasswordManager.SharedDtos;
using PasswordManager.Utils;
using Secret = PasswordManager.DAL.Entities.Secret;


namespace PasswordManager.Features.Secrets;

public class SecretService
{
    private SecretRepository _secretRepository;
    private UserResitory _userResitory;
    private SecretKeyRepository _secretKeyRepository;

    public SecretService(SecretRepository secretRepository, UserResitory userResitory, SecretKeyRepository secretKeyRepository)
    {
        this._secretRepository = secretRepository;
        this._userResitory = userResitory;
        this._secretKeyRepository = secretKeyRepository;
    }

    public async Task CreateSecret(SecretRequestCreateDto payload, Guid userId)
    {
        var user = await _userResitory.GetUserById(userId) ?? throw new ArgumentException("Usuário Inválido");
        var userSecretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId) ?? throw new ArgumentException("Chave secreta inválida");

        Secret secret = new Secret
        {
            Title = payload.Title,
            Username = payload.UserName,
            Password = payload.Password = AESHelper.Encrypt(userSecretKey.Key, payload.Password),
            User = user,
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
        var secrets = await _secretRepository.ListSecrets(userId, pagination, projection);

        return new PageableDto<SecretResponseDto>()
        {
            Items = secrets,
            Page = pagination.Page,
            Size = pagination.Size,
            TotalItems = 1,
            TotalPages = 1 / pagination.Size
        };
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
