namespace Core.Exceptions;

/// <summary>
/// Exceção lançada quando há um conflito de recursos (ex: email duplicado, recurso já existe)
/// Retorna HTTP 409 Conflict
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
    
    public ConflictException(string message, Exception innerException) : base(message, innerException) { }
}
