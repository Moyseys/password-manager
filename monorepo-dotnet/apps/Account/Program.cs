using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Features.Auth.Interfaces;
using Account.Features.Auth.Services;
using Account.Features.Users.Interfaces;
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
using SimpleMq.Config;
using SimpleMq.Extensions;
using Account.Publishers;

var builder = WebApplication.CreateBuilder(args);

//Json configs
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//AppSetting options
builder.Services
    .AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(JwtSettings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<CookiesSettings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(CookiesSettings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<MFAEmailSettings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(MFAEmailSettings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var jwtSettings = builder.Configuration.GetRequiredSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings configuration is missing");

var cookieSettings = builder.Configuration.GetRequiredSection(nameof(CookiesSettings)).Get<CookiesSettings>()
    ?? throw new InvalidOperationException("Cookies configuration is missing");

var mfaEmailSettings = builder.Configuration.GetRequiredSection(nameof(MFAEmailSettings)).Get<MFAEmailSettings>()
    ?? throw new InvalidOperationException("MFAEmailSettings configuration is missing");

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(cookieSettings);
builder.Services.AddSingleton(mfaEmailSettings);

//HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContext>();

//Interceptors
builder.Services.AddScoped<AuditInterceptor>();


//Db Context
builder.Services.AddDbContext<PasswordManagerDbContext>((serviceProvider, options) =>
{
    var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
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

//DI
builder.Services.AddScoped<PasswordHasher<User>>();

//Services
builder.Services.AddScoped<IAuthSessionService, AuthSessionService>();
builder.Services.AddScoped<IAuthCookieService, AuthCookieService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthMFAService, AuthMFAService>();

//Repositories
builder.Services.AddScoped<UserResitory>();
builder.Services.AddScoped<MFASettingsRepository>();
builder.Services.AddScoped<MFATokenRepository>();

//Exeption
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Auth
builder.Services.AddAuthenticationConfig(jwtSettings, cookieSettings.AuthCookie);

builder.Services.AddOpenApi();

//RabiitMq
var mqConfig = new NotificationMqConfig();

builder.Services.AddSimpleMQ(
    builder.Configuration,
    exchangeConfig: mqConfig,
    queueConfig: mqConfig,
    bindConfig: mqConfig
);

//Publishers
builder.Services.AddScoped<INotificationPublisher, NotificationPublisher>();


var app = builder.Build();

// Middleware 
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapScalarApiReference("doc");
app.MapOpenApi();

app.Run();
