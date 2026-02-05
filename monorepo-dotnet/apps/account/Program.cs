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

//Db Context
builder.Services.AddDbContext<PasswordManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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

//HttpContext
builder.Services.AddHttpContextAccessor();

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
