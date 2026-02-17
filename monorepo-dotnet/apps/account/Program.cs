using System.Text.Json;
using Account.Features.Auth;
using Auth.Setting;
using Core.Exceptions;
using DAL;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Account.Features.Users.Services;
using Account.Setting;
using Scalar.AspNetCore;
using Auth.Extension;
using DAL.Interceptors;
using Core.Contexts;

var builder = WebApplication.CreateBuilder(args);

//Json configs
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


builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(cookieSettings);

//HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContext>();

//Interceptors
builder.Services.AddScoped<AuditInterceptor>();


//Db Context
builder.Services.AddDbContext<PasswordManagerDbContext>((serviceProvider, options) =>
{
    var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(auditInterceptor);
});

//DI
builder.Services.AddScoped<PasswordHasher<User>>();

//Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

//Repositories
builder.Services.AddScoped<UserResitory>();

//Exeption
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Auth
builder.Services.AddAuthenticationConfig(jwtSettings, cookieSettings.AuthCookie);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapScalarApiReference("doc");
app.MapOpenApi();

app.Run();
