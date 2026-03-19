FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copiar solution e todos os projetos
COPY . .

# Build e publish do projeto Notification
WORKDIR /src/apps/Notification
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
# Instala krb5-libs necessario para Npgsql (driver PostgreSQL) funcionar com autenticacao GSSAPI/Kerberos
RUN apk add --no-cache krb5-libs
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Notification.dll"]
