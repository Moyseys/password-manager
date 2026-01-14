namespace Vaultify.Features.Secrets.Dtos.Requests;

public record SecretRequestUpdateDto(
    string? Title,
    string? Username,
    string? Password,
    string? MasterPassword,
    bool? Active
);