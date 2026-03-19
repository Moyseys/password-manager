using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Dtos.Requests;

namespace Account.Features.Auth.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Token(LoginRequestDto payload);
    void RemoveAuthCookie();
    CheckResponseDto Check();
}