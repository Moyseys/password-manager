using Microsoft.AspNetCore.Http;

namespace Core.Exceptions;

public class ExceptionDetailBuilder
{
    public int Status { get; set; } = StatusCodes.Status500InternalServerError;
    public string Title { get; set; } = "Internal server error";
    public string Detail { get; set; } = string.Empty;
    public string? Code { get; set; }

    public ExceptionDetailBuilder()
    {
    }

    private ExceptionDetailBuilder(int status, string title, string detail, string? code = null)
    {
        Status = status;
        Title = title;
        Detail = detail;
        Code = code;
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

    public ExceptionDetailBuilder SetCode(string? code)
    {
        Code = code;
        return this;
    }

    public ExceptionDetailBuilder Build()
    {
        return new ExceptionDetailBuilder(Status, Title, Detail, Code);
    }
}