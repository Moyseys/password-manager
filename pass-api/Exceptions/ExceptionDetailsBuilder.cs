namespace PasswordManager.Exceptions;
    
public class ExceptionDetailBuilder
{
    public int Status {get; set;} = StatusCodes.Status500InternalServerError;
    public string Title {get; set;} = "Internal server error";
    public string Detail {get; set;} = string.Empty;
    public string Type {get; set;} = "https://tools.ietf.org/html/rfc7231";

    public ExceptionDetailBuilder()
    {
    }

    private ExceptionDetailBuilder(int status, string title, string detail, string type)
    {
        Status = status;
        Title = title;
        Detail = detail;
        Type = type;
    }

    public ExceptionDetailBuilder SetStatus(int statusCode)
    {
        Status = statusCode;
        return this;
    }

    public ExceptionDetailBuilder SetTitle(string title)
    {
        Title = title;
        return this;
    }

    public ExceptionDetailBuilder SetDetail(string detail)
    {
        Detail = detail;
        return this;
    }

    public ExceptionDetailBuilder SetType(string type)
    {
        Type = type;
        return this;
    }

    public ExceptionDetailBuilder Build()
    {
        return new ExceptionDetailBuilder(Status, Title, Detail, Type);
    }
}