using System.Text.Json;
using Account.Setting;
using Auth.Setting;
using Auth.Extension;
using Core.Contexts;
using Core.Exceptions;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.Secrets;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Json config 
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
});

//Settings options
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
});

//AppSetting options
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings configuration is missing");

var cookieSettings = builder.Configuration.GetSection(nameof(CookiesSettings)).Get<CookiesSettings>()
        ?? throw new InvalidOperationException("Cookies configuration is missing");

//Banco
builder.Services.AddDbContext<PasswordManagerDbContext>((options) =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//HttpContext
builder.Services.AddHttpContextAccessor();

//Contexts
builder.Services.AddScoped<UserContext>();

//Repositories
builder.Services.AddScoped<SecretRepository>();
builder.Services.AddScoped<SecretKeyRepository>();
builder.Services.AddScoped<UserResitory>();

//Services
builder.Services.AddScoped<SecretService>();
builder.Services.AddScoped<SecretKeyService>();

//Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Auth
builder.Services.AddAuthenticationConfig(jwtSettings, cookieSettings.AuthCookie);

//OpenApi
builder.Services.AddOpenApi();

var app = builder.Build();

//Middleware
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapScalarApiReference("doc");
app.MapOpenApi();

app.Run();
