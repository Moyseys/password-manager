# Auth Library

Biblioteca de autenticação JWT para aplicações .NET, fornecendo serviços para geração e validação de tokens JWT, além de middleware para proteger endpoints. Utiliza o **Options Pattern** para configuração fortemente tipada e injeção de dependências.

## Estrutura

```
auth/
├── Dtos/
│   ├── TokenPayloadDto.cs   # DTO de payload do token
│   └── JwtSettingsDto.cs    # DTO de configurações JWT (legado)
├── Settings/
│   └── JwtSettings.cs       # Configurações JWT (Options Pattern)
├── Services/
│   └── TokenService.cs      # Serviço estático de geração/validação de tokens
└── Middleware/
    └── AuthMiddleware.cs    # Middleware de autenticação
```

## Funcionalidades

### TokenService

Serviço estático que fornece métodos para trabalhar com JWT:

- **GenerateTokenJwt**: Gera um token JWT a partir de dados do usuário
- **ValidateToken**: Valida e decodifica um token JWT

### AuthMiddleware

Middleware que protege rotas validando o token JWT presente no header `Authorization`.

## Instalação e Configuração

### 1. Adicionar configurações JWT ao `appsettings.json`

```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-super-segura-com-pelo-menos-32-caracteres",
    "Issuer": "PasswordManagerApp",
    "Audience": "PasswordManagerApp"
  }
}
```

> **⚠️ IMPORTANTE - Segurança:**
>
> - A `SecretKey` deve ter **no mínimo 32 caracteres**
> - **NUNCA** coloque a chave real no `appsettings.json` de produção
> - Use **User Secrets** para desenvolvimento local
> - Use **Variáveis de Ambiente** ou **Azure Key Vault** em produção

### 2. Configurar User Secrets (Desenvolvimento)

```bash
cd apps/account
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "sua-chave-super-secreta-aqui"
```

### 3. Registrar serviços no `Program.cs`

```csharp
using Auth.Setting;
using Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Registra JwtSettings usando Options Pattern
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// Adiciona o JwtSettings como singleton para o middleware
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value
);

// Registra controllers
builder.Services.AddControllers();

var app = builder.Build();

// Registra o middleware de autenticação
app.UseMiddleware<AuthMiddleware>();

// Mapeia controllers
app.MapControllers();

app.Run();
```

## Uso

### 1. Injetar JwtSettings em um Service

Use `IOptions<JwtSettings>` para injeção no construtor:

```csharp
using Auth.Setting;
using Auth.Services;
using Auth.Dtos;
using Microsoft.Extensions.Options;

public class AuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string Login(User user)
    {
        var payload = new TokenPayloadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

        return TokenService.GenerateTokenJwt(_jwtSettings, payload);
    }
}
```

### 2. Gerar Token JWT

```csharp
using Auth.Services;
using Auth.Dtos;
using Auth.Setting;

// Em um controller ou service com JwtSettings injetado
var payload = new TokenPayloadDto
{
    Id = Guid.NewGuid(),
    Name = "João Silva",
    Email = "joao@example.com"
};

var token = TokenService.GenerateTokenJwt(_jwtSettings, payload);
```

### 3. Validar Token JWT

```csharp
using Auth.Services;
using System.Security.Claims;

var principal = TokenService.ValidateToken(_jwtSettings, token);

if (principal != null)
{
    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
    var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
}
```

### 4. Usar o Middleware

O middleware automaticamente:

1. Extrai o token do header `Authorization: Bearer <token>`
2. Valida o token usando as configurações JWT
3. Adiciona as claims do usuário ao `HttpContext.User`
4. Lança exceção se o token for inválido ou ausente

Após passar pelo middleware, você pode acessar as informações do usuário no controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { userId, userName, userEmail });
    }
}
```

## Exemplo Completo

### Program.cs

```csharp
using Auth.Setting;
using Core.Middleware;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configura Options Pattern para JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// Registra JwtSettings como singleton para middleware
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value
);

// Registra outros serviços
builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();

var app = builder.Build();

// Middleware de autenticação
app.UseMiddleware<AuthMiddleware>();

app.MapControllers();
app.Run();
```

### AuthService.cs

```csharp
using Auth.Setting;
using Auth.Services;
using Auth.Dtos;
using Microsoft.Extensions.Options;

public class AuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateUserToken(Guid userId, string name, string email)
    {
        var payload = new TokenPayloadDto
        {
            Id = userId,
            Name = name,
            Email = email
        };

        return TokenService.GenerateTokenJwt(_jwtSettings, payload);
    }
}
```

## Tratamento de Erros

O middleware lança as seguintes exceções:

- **InvalidDataException**: Token ausente ou inválido
- **UnauthorizedAccessException**: Token expirado
- **SecurityTokenException**: Problemas na validação do token

Recomenda-se configurar um middleware de tratamento global de exceções:

```csharp
app.UseExceptionHandler("/error");
```

## Vantagens do Options Pattern

✅ **Fortemente tipado**: Erros de configuração detectados em tempo de compilação  
✅ **Validação automática**: Propriedades `required` garantem configurações obrigatórias  
✅ **Testável**: Fácil de mockar em testes unitários  
✅ **Hot-reload**: Suporta atualização de configurações sem reiniciar  
✅ **Injeção de dependência**: Segue as melhores práticas do .NET  
✅ **Separação de responsabilidades**: Configuração isolada do código de negócio

## Segurança em Produção

### Variáveis de Ambiente

```bash
export JwtSettings__SecretKey="sua-chave-producao"
export JwtSettings__Issuer="ProductionApp"
```

### Azure App Service

Configure nas Application Settings:

- `JwtSettings:SecretKey`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`

### Docker

```dockerfile
ENV JwtSettings__SecretKey="sua-chave-producao"
```

## Troubleshooting

### Erro: "SecretKey is required"

Verifique se `JwtSettings:SecretKey` está configurado no `appsettings.json` ou nas variáveis de ambiente.

### Erro: "InvalidToken"

- Verifique se o token está no formato `Bearer <token>`
- Confirme que a `SecretKey` é a mesma usada na geração e validação
- Verifique se o token não expirou (validade padrão: 1 hora)

### Middleware não está sendo executado

Certifique-se de que `app.UseMiddleware<AuthMiddleware>()` está **antes** de `app.MapControllers()`.

```csharp
[HttpGet("me")]
public IActionResult GetCurrentUser()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userName = User.FindFirst(ClaimTypes.Name)?.Value;
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

    return Ok(new { userId, userName, userEmail });
}
```

## DTOs

### UserDto

```csharp
public class UserDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
```

### JwtSettingsDto

```csharp
public class JwtSettingsDto
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
```

## Exceções

O middleware lança `InvalidDataException` nos seguintes casos:

- Header `Authorization` não encontrado
- Token inválido ou mal formatado
- Token expirado
- Token com assinatura inválida

## Segurança

⚠️ **Importante:**

- Use chaves secretas fortes (mínimo 256 bits)
- Nunca commite chaves secretas no repositório
- Use variáveis de ambiente em produção
- Configure HTTPS em produção

## Dependências

- `Microsoft.AspNetCore.Http`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.DependencyInjection`
- `System.IdentityModel.Tokens.Jwt`
- `Microsoft.IdentityModel.Tokens`

## Exemplo Completo

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

// Adicionar middleware de autenticação
app.UseMiddleware<AuthMiddleware>();

app.MapControllers();
app.Run();
```

```csharp
// LoginController.cs
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Validar credenciais (exemplo simplificado)
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Name = request.Username,
            Email = request.Email
        };

        var token = AuthService.GenerateTokenJwt(_configuration, user);

        return Ok(new { token });
    }
}
```

## Licença

Este projeto faz parte de um monorepo privado.
