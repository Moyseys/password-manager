namespace Core.Exceptions;

/// <summary>
/// Exception thrown when there is a resource conflict (e.g., duplicate email, resource already exists)
/// Returns HTTP 409 Conflict
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }
}
