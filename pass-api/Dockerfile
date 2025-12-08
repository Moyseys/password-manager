FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as runtime
WORKDIR /app

COPY --from=build ./app/dist .
EXPOSE 8080

ENTRYPOINT [ "dotnet", "password-manager.dll" ]