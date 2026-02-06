namespace Vaultify.Features.Secrets.Dtos.Response;

public record SecretResponseUpdateDto(
    string Title,
    string Username,
    string Website,
    string CipherPassword,
    string IV,
    bool Active
);