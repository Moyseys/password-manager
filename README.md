# Password Manager

Projeto simples de exemplo para gerenciar segredos — contém uma API em .NET 9 (`pass-api`), um cliente Angular (`pass-client`) e uma gateway (`pass-gw`). Este README traz instruções rápidas e amigáveis para rodar o projeto localmente ou via Docker.

**Resumo rápido:**

- **API:** `pass-api` (ASP.NET Core, EF Core + Npgsql)
- **Client:** `pass-client` (Angular)
- **Gateway:** `pass-gw` (Krakend)
- **Banco de dados:** PostgreSQL (configurado no `docker-compose.yml`)

**Pré-requisitos**

- .NET 9 SDK (para desenvolvimento da API)
- Node.js (v18+) e `npm` (para o client Angular)
- Docker e `docker-compose` (recomendado para quickstart)
- (Opcional) `dotnet-ef` para aplicar migrations localmente

---

## 1) Quickstart com Docker (recomendado)

1. Crie um arquivo `.env` na raiz do repositório com os valores abaixo (exemplo):

```
POSTGRES_PASSWORD=postgres
CONNECTIONSTRING_POSTGRES=Host=db;Database=password_manager;Username=postgres;Password=postgres
JWT_SECRET=nG426ht60vTZ4DC1ITswuJjij8yiVoXROpxNHv9n1J4=
```

2. Suba os serviços (banco + API + gateway):

```bash
docker-compose up --build
```

3. Acesse:

- API: http://localhost:5000
- Gateway: http://localhost:8080

Observação: o serviço `db` no `docker-compose` usa a variável `POSTGRES_PASSWORD` do `.env`.

---

## 2) Rodando localmente (sem Docker)

API (dotnet):

1. Garanta que um PostgreSQL esteja rodando (ex.: `localhost:5432`) e crie um banco `password_manager` com usuário `postgres`.
   - Você pode rapidamente criar um container postgres local:

```bash
docker run --name pm-db -e POSTGRES_DB=password_manager -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:15
```

2. Exporte variáveis de ambiente necessárias (ou ajuste `appsettings.Development.json`):

```bash
cd pass-api
export ConnectionStrings__Postgres="Host=localhost;Database=password_manager;Username=postgres;Password=postgres"
export JwtSettings__SecretKey="nG426ht60vTZ4DC1ITswuJjij8yiVoXROpxNHv9n1J4="
```

3. Restaurar e executar:

```bash
dotnet restore
dotnet run --urls=http://localhost:5000
```

4. Aplicar migrations (se desejar criar as tabelas via EF):

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
dotnet ef database update
```

Client (Angular):

1. Abra outra janela/terminal e rode:

```bash
cd pass-client
npm install
npm start
```

2. O cliente Angular normalmente abre em `http://localhost:4200`. Ajuste o endpoint da API em `pass-client/src/environments/environment.ts` ou via `app.config.ts` caso necessário.

---

## 3) Variáveis e configuração

- `pass-api` lê as connection strings a partir de `ConnectionStrings:Postgres` (no `appsettings.json` ou via variáveis de ambiente `ConnectionStrings__Postgres`).
- `JWT` e outros segredos podem ser fornecidos via `JwtSettings__SecretKey` em variáveis de ambiente ou `.env` quando usando Docker.

---

## 4) Dicas e troubleshooting

- Se a API não conseguir conectar ao banco, verifique se o Postgres está rodando e se a connection string está correta.
- Para development é útil rodar o banco via Docker (comandos acima) para evitar diferenças de ambiente.
- Se usar Docker e o compose falhar devido a variáveis, verifique que o arquivo `.env` está no diretório raiz.
