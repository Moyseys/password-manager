using System.Text.Json;
using Account.Features.Auth;
using Auth.Setting;
using Auth.Middleware;
using Core.Exceptions;
using DAL;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Account.Features.Users.Services;

var builder = WebApplication.CreateBuilder(args);

//Json configs
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
    options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
});

//JwtConfig
builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() 
        ?? throw new InvalidOperationException("JwtSettings configuration is missing")
);

//Dp Context
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

var app = builder.Build();

var publicRouter = new Dictionary<string, string[]>() {
    { "/api/v1/auth", ["POST"] },
    { "/api/v1/users", ["POST"] }
};

app.UseExceptionHandler();
app.UseWhen(context => {
    var path = context.Request.Path;

    if (path.StartsWithSegments("/scalar") || 
    path.StartsWithSegments("/openapi"))
    {
        return false; 
    }

    if (publicRouter.TryGetValue(path, out var methods_path))
        if (methods_path.Contains(context.Request.Method)) 
            return false;

    return true;
    },
    appBuilder => appBuilder.UseMiddleware<AuthMiddleware>()
);

app.MapControllers();

app.Run();
