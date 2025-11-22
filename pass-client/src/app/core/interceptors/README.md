# Auth Interceptor

## Descrição

Interceptor HTTP que adiciona automaticamente o header `Authorization` com o Bearer token em todas as requisições HTTP.

## Funcionamento

1. **Obtenção do Token**: O interceptor busca o token de autenticação do cookie configurado (`sso-token`)
2. **Validação**: Se não houver token, a requisição continua sem modificações
3. **Injeção do Header**: Se houver token, o header `Authorization: Bearer {token}` é adicionado automaticamente
4. **Requisição**: A requisição modificada é enviada ao servidor

## Configuração

O interceptor está registrado globalmente em `app.config.ts`:

```typescript
provideHttpClient(withInterceptors([authInterceptor]));
```

## Uso

O interceptor funciona automaticamente em **todas as requisições HTTP** feitas através do `HttpClient` do Angular.

### Exemplo de Requisição

```typescript
// Antes (sem interceptor) - você precisaria adicionar o header manualmente
this.http.get('/api/secrets', {
  headers: {
    Authorization: `Bearer ${token}`,
  },
});

// Agora (com interceptor) - o header é adicionado automaticamente
this.http.get('/api/secrets');
```

## Estrutura do Header

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Benefícios

- ✅ **Centralizado**: Lógica de autenticação em um único lugar
- ✅ **Automático**: Não precisa adicionar header manualmente em cada requisição
- ✅ **Consistente**: Garante que todas as requisições usem o mesmo padrão
- ✅ **Manutenível**: Fácil de modificar ou estender no futuro
- ✅ **Type-safe**: Usa a API funcional mais moderna do Angular

## Dependências

- `CookieService`: Para obter o token do cookie
- `environment.cookies.token`: Configuração do nome do cookie

## Notas

- O interceptor usa a API funcional (`HttpInterceptorFn`) do Angular 17+
- O token é obtido do cookie em cada requisição (sempre atualizado)
- Se o token não existir, a requisição é enviada normalmente sem o header
