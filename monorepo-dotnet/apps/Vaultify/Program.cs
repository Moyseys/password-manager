using System.Text.Json;
using Auth.Middleware;
using Auth.Setting;
using Core.Contexts;
using Core.Exceptions;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Json config 
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
});

//JwtSetting
builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() 
        ?? throw new InvalidOperationException("JwtSettings configuration is missing")
);


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

var app = builder.Build();

//Middleware
app.UseExceptionHandler();
app.UseMiddleware<AuthMiddleware>();
app.MapControllers();

app.Run();
