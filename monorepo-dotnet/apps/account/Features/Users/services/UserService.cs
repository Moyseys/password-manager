using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using DAL.Repositories;
using DAL.Entities;
using Core.Exceptions;
using SharedDto.Dtos;
using SharedDto.Mappers;

namespace Account.Features.Users.Services;

public class UserService(UserResitory userResitory, PasswordHasher<User> passwordHasher)
{
    private readonly UserResitory userResitory = userResitory;
    private readonly PasswordHasher<User> passwordHasher = passwordHasher;

    public async Task CreateUser(CreateUserDto payload)
    {
        var user = await userResitory.GetUserByEmailAsync(payload.Email);
        if (user != null) throw new ConflictException("User already exists");

        var newUser = payload.ToUser();
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, newUser.Password);

        var validationUser = ValidateCreateUser(newUser);
        if (validationUser != null && validationUser.Count() > 0)
            throw new InvalidDataException(string.Join("; ", validationUser.Select(v => v.ErrorMessage)));

        await userResitory.AddAsync(newUser);
        return;
    }

    public List<ValidationResult> ValidateCreateUser(User user)
    {
        var validationContext = new ValidationContext(user);
        var result = new List<ValidationResult>();

        Validator.TryValidateObject(user, validationContext, result, validateAllProperties: true);
        return result;
    }
}