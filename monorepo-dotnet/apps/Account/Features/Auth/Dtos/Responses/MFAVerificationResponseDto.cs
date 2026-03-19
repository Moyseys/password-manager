namespace Account.Features.Auth.Dtos.Responses;

public class MFAVerificationResponseDto
{
    public required bool IsValid { get; set; }
    public required string Message { get; set; }
}
