namespace Core.Exceptions;

public class MFAVerificationException : Exception
{
    public int? StatusCode { get; set; }

    public MFAVerificationException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }
}
