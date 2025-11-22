using Microsoft.AspNetCore.Http.HttpResults;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.SecretKey.Dtos;
using SecretKeyEntity = PasswordManager.DAL.Entities.SecretKey;

namespace PasswordManager.Features.SecretKey;

public class SecretKeyService
{
    private SecretKeyRepository secretKeyRepository;
    private UserResitory userResitory;
    
    public SecretKeyService(SecretKeyRepository secretKeyRepository, UserResitory userResitory)
    {   
        this.secretKeyRepository = secretKeyRepository;
        this.userResitory = userResitory;
    }

    async public Task<SecretKeyResponseDto> Create(SecretKeyRequestDto payload, Guid userId)
    {
        var existByUserId = await secretKeyRepository.ExistByUserId(userId);
        System.Console.WriteLine(existByUserId);
        if (existByUserId)
        {
            throw new BadHttpRequestException("A secret key already exists for this user.");
        }
            
        var secretKey = new SecretKeyEntity()
        {
            Key = payload.Key,
            UserId = userId,
            User = null!
        };
        await secretKeyRepository.AddAsync(secretKey);
        Console.WriteLine(secretKey.Key, secretKey.UserId);
        return new SecretKeyResponseDto { key = secretKey.Key, userId = secretKey.UserId.ToString() };
    }
}