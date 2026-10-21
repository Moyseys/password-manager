using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Setting;
using Auth.Setting;
using Auth.Extension;
using Microsoft.AspNetCore.Authorization;
using Core.Contexts;
using Core.Exceptions;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.Secrets;
using Vaultify.Features.Categories;
using Scalar.AspNetCore;
using Vaultify.Features.Dashboard;
using DAL.Interceptors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Json config 
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

//HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContext>();

//Interceptors
builder.Services.AddScoped<AuditInterceptor>();

//Banco
builder.Services.AddDbContext<PasswordManagerDbContext>((ServiceProvider, options) =>
{
    var auditInterceptor = ServiceProvider.GetRequiredService<AuditInterceptor>();
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null
                );
                npgsqlOptions.CommandTimeout(30);
            })
        .AddInterceptors(auditInterceptor);
});

//Repositories
builder.Services.AddScoped<SecretRepository>();
builder.Services.AddScoped<SecretKeyRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<UserResitory>();
builder.Services.AddScoped<DashboardRepository>();

//Services
builder.Services.AddScoped<SecretService>();
builder.Services.AddScoped<SecretKeyService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<DashboardService>();

//Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Auth
builder.Services.AddAuthenticationConfig(jwtSettings, cookieSettings.AuthCookie);

//Authorization: exige usuário autenticado por padrão em TODOS os endpoints
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

//CORS (dev: reflete a origem e permite credenciais)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//OpenApi
builder.Services.AddOpenApi();


var app = builder.Build();

//Middlewares
app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapScalarApiReference("doc");
app.MapOpenApi();

app.Run();
