using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using DAL.Repositories;
using DAL.Entities;
using Core.Utils;
using Core.Exceptions;
using SharedDto.Dtos;
using SharedDto.Mappers;

namespace Account.Features.Users.Services;
public class UserService(UserResitory userResitory, PasswordHasher<User> passwordHasher)
{
    private readonly UserResitory userResitory = userResitory;
    private readonly PasswordHasher<User> passwordHasher = passwordHasher;

    public async Task<CreateUserDto> CreateUser(CreateUserDto payload)
    {
        var user = await userResitory.GetUserByEmailAsync(payload.Email);
        if (user != null) throw new ConflictException("User already exists");
        
        var (secretKey, masterPasswordSalt) = GenerateVaultKeys(payload.MasterPassword);

        var newUser = payload.ToUser(masterPasswordSalt);
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, newUser.Password);
        
        var validationUser = ValidateCreateUser(newUser);
        if(validationUser != null && validationUser.Count() > 0)
            throw new InvalidDataException(string.Join("; ", validationUser.Select(v => v.ErrorMessage)));

        var secretKeyEntity = new SecretKey
        {
            Key = secretKey,
            User = newUser,
            UserId = newUser.Id
        };

        await userResitory.AddWithSecretKeyAsync(newUser, secretKeyEntity);
        return payload;
    }

    private (byte[] secretKey, byte[] salt) GenerateVaultKeys(string masterPassword)
    {
                
        byte[] vaultKeyBytes = Aes.Create().Key;
        byte[] masterPasswordSalt = new byte[32];
        RandomNumberGenerator.Fill(masterPasswordSalt);
        byte[] masterDerived = DeriveHelper.RFC2898(masterPassword, masterPasswordSalt);

        var secretKey = AESHelper.Encrypt(masterDerived, vaultKeyBytes);

        return (secretKey, masterPasswordSalt);
    }

    public List<ValidationResult> ValidateCreateUser(User user)
    {
        var validationContext = new ValidationContext(user);
        var result = new List<ValidationResult>();

        Validator.TryValidateObject(user, validationContext, result, validateAllProperties: true);
        return result;
    }
}