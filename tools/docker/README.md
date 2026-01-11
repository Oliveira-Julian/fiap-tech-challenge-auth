# Docker Setup para FoodChallenge.Auth

Este diretório contém a configuração Docker para executar o servidor OAuth2/OIDC da aplicação de autenticação.

## Pré-requisitos

- Docker instalado
- Docker Compose instalado
- PostgreSQL existente em execução (container `foodchallenge_db`)

## Como Usar

### 1. Criar o banco de dados `foodchallenge_auth`

Antes de iniciar a API, você precisa criar o banco de dados no PostgreSQL existente:

```bash
# Conectar ao PostgreSQL existente
psql -h localhost -U postgres -d foodchallenge_db -f tools/docker/init-db.sql
```

Ou manualmente via psql:

```bash
psql -h localhost -U postgres
CREATE DATABASE foodchallenge_auth
    WITH
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.UTF-8'
    LC_CTYPE = 'en_US.UTF-8'
    TEMPLATE = template0;
\q
```

### 2. Iniciar a Auth API

```bash
cd tools/docker
docker-compose up -d
```

Isso irá:
- ✅ Compilar a imagem da Auth API
- ✅ Iniciar a API na porta 5000 conectada ao banco `foodchallenge_auth`

### 3. Verificar o status

```bash
docker-compose ps
```

A API deve estar em estado `Up`.

### 4. Acessar a API

- **OAuth2 Token Endpoint**: `http://localhost:5000/connect/token`
- **OAuth2 Introspection Endpoint**: `http://localhost:5000/connect/introspect`

### 5. Parar os serviços

```bash
docker-compose down
```

## Variáveis de Ambiente

As variáveis estão configuradas no arquivo `.env`:

- `POSTGRES_USER`: Usuário do PostgreSQL
- `POSTGRES_PASSWORD`: Senha do PostgreSQL
- `POSTGRES_HOST`: Host do PostgreSQL (foodchallenge_db)
- `POSTGRES_DB`: Nome do banco de dados (foodchallenge_auth)
- `ConnectionStrings__PostgreSQL`: Connection string completa
- `ASPNETCORE_ENVIRONMENT`: Environment da aplicação (Development/Production)

## Exemplo de Teste

```bash
curl -X POST http://localhost:5000/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials&client_id=190abe6d-8d8a-4cc5-9d9c-22e599525c9f&client_secret=948a0a2e-5072-4739-818c-6f53ddc2a192&scope=orders.read"
```

## Logs

Para visualizar os logs da API:

```bash
docker-compose logs -f foodchallenge_auth_api
```

## Troubleshooting

### Erro de conexão com PostgreSQL

Se a API não conseguir conectar ao PostgreSQL:

1. Verifique se `foodchallenge_db` está em execução:
   ```bash
   docker ps | grep foodchallenge_db
   ```

2. Verifique o `.env` para credenciais corretas

3. Teste a conexão manualmente:
   ```bash
   psql -h localhost -U postgres -d foodchallenge_auth
   ```

### Banco de dados não existe

Se receber erro "database foodchallenge_auth does not exist", execute novamente:

```bash
psql -h localhost -U postgres -f tools/docker/init-db.sql
```

## Arquitetura

```
┌─────────────────────────────────┐
│   FoodChallenge.Auth.Api        │
│   (ASP.NET Core 9 - Port 5000)  │
└──────────────┬──────────────────┘
               │
               ↓
┌─────────────────────────────────┐
│   PostgreSQL (foodchallenge_db) │
│   ├── foodchallenge_auth        │
│   └── (outros bancos...)        │
└─────────────────────────────────┘
```
