using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using SecretKeyEntity = PasswordManager.DAL.Entities.SecretKey;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Users.Dtos;
using PasswordManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Features.Users.Services
{
    public class UserService
    {
        private  UserResitory userResitory;
        private  SecretKeyRepository secretKeyRepository;
        private readonly PasswordHasher<User> passwordHasher;

        public UserService(UserResitory userResitory, SecretKeyRepository secretKeyRepository, PasswordHasher<User> passwordHasher)
        {
            this.userResitory = userResitory;
            this.passwordHasher = passwordHasher;
            this.secretKeyRepository = secretKeyRepository;
        }

        public async Task<CreateUserDto> CreateUser(CreateUserDto payload)
        {
            var user = await userResitory.GetUserByEmailAsync(payload.Email);
            if (user != null) throw new InvalidDataException("User alredy exist");
            
            
            byte[] vaultKeyBytes = Aes.Create().Key;
            byte[] masterPasswordSalt = new byte[32];
            RandomNumberGenerator.Fill(masterPasswordSalt);
            byte[] masterDerived = DeriveHelper.RFC2898(payload.MasterPassword, masterPasswordSalt);

            var secretKey = AESHelper.Encrypt(masterDerived, vaultKeyBytes);

            var newUser = new User
            {
                Email = payload.Email,
                Name = payload.Name,
                Password = payload.Password,
                PasswordHash = "",
                MasterPasswordSalt = masterPasswordSalt
            };
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, newUser.Password);
            
            var validationUser = ValidateCreateUser(newUser);
            if(validationUser != null && validationUser.Count() > 0)
                throw new InvalidDataException(string.Join("; ", validationUser.Select(v => v.ErrorMessage)));

            await userResitory.AddAsync(newUser);

            var sk = new SecretKeyEntity
            {
                Key = secretKey,
                UserId = newUser.Id
            };


            await secretKeyRepository.AddAsync(sk);
            return payload;
        }

        public List<ValidationResult> ValidateCreateUser(User user)
        {
            var validationContext = new ValidationContext(user);
            var result = new List<ValidationResult>();

            Validator.TryValidateObject(user, validationContext, result, validateAllProperties: true);
            return result;
        }
    }
}