using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using DAL.Repositories;
using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Dtos.Requests;
using Auth.Services;
using Auth.Dtos;
using Auth.Setting;

namespace Account.Features.Auth;

public class AuthService(UserResitory userResitory, PasswordHasher<User> passwordHasher, JwtSettings jwtSettings)
{
    private readonly PasswordHasher<User> passwordHasher = passwordHasher;
    private readonly UserResitory userResitory = userResitory;
    private readonly JwtSettings jwtSettings = jwtSettings;

    public async Task<LoginResponseDto> Token(LoginRequestDto payload)
    {
        if (string.IsNullOrEmpty(payload.Email) || string.IsNullOrEmpty(payload.Password))
            throw new InvalidDataException("Invalid E-mail or Password!");

        User? user = await userResitory.GetUserByEmailAsync(payload.Email) 
            ?? throw new InvalidDataException("E-mail not registred!");
        
        var verifyPass = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, payload.Password);
        if(verifyPass != PasswordVerificationResult.Success) throw new InvalidDataException("Invalid Password!");

        return new LoginResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Token = GenToken(user)
        };
    }

    private string GenToken(User user)
    {
        var payload = new TokenPayloadDto(){ Id = user.Id, Email = user.Email, Name = user.Name};
        
        return TokenService.GenerateTokenJwt(jwtSettings, payload);
    }
}