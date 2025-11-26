using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PasswordManager.DAL;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Auth;
using PasswordManager.Features.SecretKey;
using PasswordManager.Features.Secrets;
using PasswordManager.Features.Users.Services;
using PasswordManager.Middleware;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers() //Configura construtores e JSON
    .AddJsonOptions((options) =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Converte propriedade para camelcase
        options.JsonSerializerOptions.WriteIndented = false; // Produz JSON conpacto
    });
builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddDbContext<PasswordManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))); //Configuração do banco -> EF

//Repositories
builder.Services.AddScoped<UserResitory>();
builder.Services.AddScoped<SecretRepository>();
builder.Services.AddScoped<SecretKeyRepository>();

//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SecretService>();
builder.Services.AddScoped<SecretKeyService>();

//Entities
builder.Services.AddScoped<PasswordHasher<User>>();

//Doc
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//TODO Adicionar Métodos para as rotas
var publicRouter = new Dictionary<string, string[]>() {
    { "/api/v1/auth", ["POST"] },
    { "/api/v1/users", ["POST"] }
};

//Middlewares
app.UseWhen(
    context =>
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments("/scalar") || 
            path.StartsWithSegments("/openapi"))
        {
            return false; 
        }

        if (publicRouter.TryGetValue(path, out var methods_path))
        {   
            if (methods_path.Contains(context.Request.Method)) return false;
        }
        return true;
    }, 
    appBuilder => appBuilder.UseMiddleware<AuthMiddleware>()
);

app.MapControllers();
app.UseAuthentication();
app.Run();