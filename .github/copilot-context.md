# GitHub Copilot Context - Password Manager

Este arquivo fornece contexto para o GitHub Copilot entender a estrutura e padrões do projeto Password Manager.

## Visão Geral do Projeto

Sistema de gerenciamento de senhas com arquitetura de microserviços composto por:

- **API Backend**: ASP.NET Core 9.0 (`pass-api`)
- **Frontend**: Angular 20+ (`pass-client`)
- **API Gateway**: KrakenD (`pass-gw`)
- **Registry**: Container registry (`pass-registry`)
- **Servidor**: Nginx (`pass-server`)
- **Banco de Dados**: PostgreSQL

## Estrutura da API (pass-api)

### Tecnologias

- .NET 9.0
- Entity Framework Core 9.0
- PostgreSQL (Npgsql)
- JWT Authentication
- Scalar para documentação API

### Padrões de Arquitetura

- **Clean Architecture** com separação em camadas
- **Repository Pattern** para acesso a dados
- **Feature Folders** organizados por funcionalidade
- **Middleware personalizado** para autenticação
- **Global Exception Handling**

### Estrutura de Pastas

```
pass-api/
├── Contexts/           # Contextos de autenticação e autorização
├── DAL/               # Data Access Layer
│   ├── Entities/      # Entidades do banco de dados
│   ├── Repositories/  # Implementação do Repository Pattern
│   ├── Dtos/         # Data Transfer Objects
│   ├── Projections/  # Projeções para queries
│   ├── Extensions/   # Extensões do EF Core
│   └── Interceptors/ # Interceptors do EF Core
├── Features/         # Organizados por domínio/funcionalidade
│   ├── Auth/         # Autenticação e autorização
│   ├── Users/        # Gerenciamento de usuários
│   ├── Secrets/      # Gerenciamento de segredos/senhas
│   └── SecretKey/    # Chaves de criptografia
├── Middleware/       # Middleware personalizado
├── Exceptions/       # Tratamento global de exceções
├── Extensions/       # Métodos de extensão
├── Mappers/         # Mapeamento entre DTOs e entidades
├── Utils/           # Utilitários (criptografia, etc.)
└── SharedDtos/      # DTOs compartilhados
```

### Convenções de Nomenclatura

- **Controllers**: Sufixo `Controller` (ex: `UsersController`)
- **Services**: Sufixo `Service` (ex: `UserService`)
- **Repositories**: Sufixo `Repository` (ex: `UserRepository`)
- **DTOs**: Sufixo `Dto` (ex: `UserDto`)
- **Entities**: Nome do domínio (ex: `User`, `Secret`)

### Padrões de Código

- **Nullable reference types** habilitado
- **Implicit usings** habilitado
- **Repository + Unit of Work** para acesso a dados
- **Dependency Injection** para todos os serviços
- **Middleware pipeline** customizado
- **JWT Bearer Authentication**

## Frontend (pass-client)

### Tecnologias

- Angular 20+
- TypeScript
- Angular CDK
- Prettier para formatação

### Estrutura

- Componentes organizados por feature
- Serviços para comunicação com API
- Guards para proteção de rotas
- Interceptors para autenticação

## Infraestrutura

### Docker

- **Multi-container setup** com docker-compose
- **Environment variables** para configuração
- **Resource limits** definidos
- **Health checks** configurados

### Banco de Dados

- **PostgreSQL** como SGBD principal
- **Entity Framework Migrations** para versionamento
- **Connection strings** via variáveis de ambiente

### Gateway (KrakenD)

- **API Gateway** centralizando requisições
- **Rate limiting** e **CORS** configurados
- **Load balancing** entre instâncias

## Convenções de Desenvolvimento

### Git

- Branch principal: `main`
- Feature branches: `feature/nome-da-feature`
- Commits em português
- Pull requests obrigatórios

### Configuração

- **Environment variables** para diferentes ambientes
- **appsettings.json** para configurações .NET
- **.env** file para variáveis do Docker

### Segurança

- **JWT tokens** para autenticação
- **Criptografia AES** para senhas (AESHelper.util.cs)
- **Environment variables** para secrets
- **HTTPS** em produção

### APIs

- **RESTful** endpoints
- **JSON** como formato padrão
- **Scalar** para documentação
- **Pagination** com `PageableDto` e `PaginationDto`

## Comandos Úteis

```bash
# Desenvolvimento local
dotnet run --project pass-api
npm start --prefix pass-client

# Docker
docker-compose up --build
docker-compose down

# Migrations
dotnet ef migrations add NomeDaMigration --project pass-api
dotnet ef database update --project pass-api
```

## Exemplo de Estrutura de Feature

Quando criar uma nova feature, seguir este padrão:

```
Features/
└── NovaFeature/
    ├── Controllers/
    │   └── NovaFeatureController.cs
    ├── Services/
    │   └── NovaFeatureService.cs
    ├── Dtos/
    │   ├── NovaFeatureDto.cs
    │   └── CreateNovaFeatureDto.cs
    └── Validators/
        └── NovaFeatureValidator.cs
```

## Contexto para Copilot

Quando sugerir código:

1. **Siga os padrões arquiteturais** estabelecidos
2. **Use dependency injection** para serviços
3. **Implemente Repository Pattern** para acesso a dados
4. **Adicione tratamento de exceções** apropriado
5. **Use DTOs** para transferência de dados
6. **Mantenha separação de responsabilidades**
7. **Adicione comentários** em português quando necessário
8. **Use async/await** para operações I/O
9. **Valide inputs** nos controllers
10. **Mantenha consistência** com código existente
