using Microsoft.AspNetCore.Http;

namespace Core.Exceptions;
    
public class ExceptionDetailBuilder
{
    public int Status {get; set;} = StatusCodes.Status500InternalServerError;
    public string Title {get; set;} = "Internal server error";
    public string Detail {get; set;} = string.Empty;

    public ExceptionDetailBuilder()
    {
    }

    private ExceptionDetailBuilder(int status, string title, string detail)
    {
        Status = status;
        Title = title;
        Detail = detail;
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

    public ExceptionDetailBuilder Build()
    {
        return new ExceptionDetailBuilder(Status, Title, Detail);
    }
}