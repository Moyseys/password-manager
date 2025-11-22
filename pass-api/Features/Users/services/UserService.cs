using Microsoft.AspNetCore.Identity;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Users.Dtos;

namespace PasswordManager.Features.Users.Services
{
    public class UserService
    {
        private  UserResitory userResitory;
        private readonly PasswordHasher<User> passwordHasher;

        public UserService(UserResitory userResitory, PasswordHasher<User> passwordHasher)
        {
            this.userResitory = userResitory;
            this.passwordHasher = passwordHasher;

        }

        public async Task<CreateUserDto> CreateUser(CreateUserDto payload)
        {
            var user = await userResitory.GetUserByEmailAsync(payload.Email);
            if (user != null) return payload;

            var newUser = new User
            {
                Email = payload.Email,
                Name = payload.Name,
                Password = payload.Password,
                PasswordHash = ""
            };
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, newUser.Password);

            await userResitory.AddAsync(newUser);
            return payload;
        }
    }
}