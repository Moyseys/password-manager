namespace Core.Exceptions;

public class MFAVerificationException : Exception
{
    public int? StatusCode { get; set; }
    public MFAErrorCode? Code { get; set; }

    public MFAVerificationException(string message, MFAErrorCode? code = null, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }
}
