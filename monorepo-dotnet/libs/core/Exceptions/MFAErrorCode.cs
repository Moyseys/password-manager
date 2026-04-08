namespace Core.Exceptions;

public enum MFAErrorCode
{
    SettingsNotFound,
    ActiveTokenAlreadyExists,
    TokenNotFoundOrUsed,
    TokenExpired,
    TokenBlocked,
    InvalidToken
}