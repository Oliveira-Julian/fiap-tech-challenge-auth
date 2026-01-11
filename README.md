# 🔐 FoodChallenge Auth API

**FoodChallenge Auth** é um servidor de autenticação OAuth2/OpenID Connect robusto, escalável e seguro, desenvolvido em **.NET 9**. Fornece um serviço de autenticação centralizado para múltiplos microserviços utilizando **Entity Framework Core**, **PostgreSQL** e **Docker** para orquestração.

---
## 📚 Índice

- [🔧 Visão Geral da Arquitetura](#-visão-geral-da-arquitetura)
- [🗂️ Estrutura do Projeto](#-estrutura-do-projeto)
- [🚀 Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [▶️ Como Executar](#-como-executar)
  - [🐳 Subindo com Docker](#-subindo-com-docker)
  - [🔗 APIs Disponíveis](#-apis-disponíveis)
- [📡 Endpoints](#-endpoints)
- [🔐 Segurança](#-segurança)
- [📖 Documentação Adicional](#-documentação-adicional)

---

## 🔧 Visão Geral da Arquitetura

A aplicação foi construída seguindo os princípios da Clean Architecture, com o objetivo de manter o núcleo da lógica de autenticação isolado de detalhes de implementação e tecnologias externas. Esse modelo proporciona:

- 🔁 **Alta coesão e baixo acoplamento**
- 🧪 **Facilidade de testes unitários e de integração**
- 🚀 **Manutenção e evolução facilitadas**
- ♻️ **Substituição simples de tecnologias externas sem impacto no domínio**

A estrutura implementa os padrões **OAuth2** e **OpenID Connect** através da biblioteca **OpenIddict**, permitindo que outras aplicações e microserviços se autentiquem de forma segura e consistente.

### Recursos Principais

- ✅ **OAuth2 + OpenID Connect** com OpenIddict 7.2.0
- ✅ **Fluxo Client Credentials** para comunicação serviço-a-serviço
- ✅ **Endpoints OAuth2**:
  - `/connect/token` - Obter tokens de acesso
  - `/connect/introspect` - Validar e inspecionar tokens
- ✅ **Gerenciamento de Escopos (Scopes)** customizáveis
- ✅ **Entity Framework Core 9** com PostgreSQL 12+
- ✅ **Injectação de Dependências** nativa do ASP.NET Core
- ✅ **Seeding automático** de dados iniciais (clientes e escopos)
- ✅ **Logging estruturado** e extensível
- ✅ **Validação de entrada** robusta

---

## 🗂️ Estrutura do Projeto

```bash
./
├─ src/
│  └─ FoodChallenge.Auth/
│     ├─ FoodChallenge.Auth.Api/
│     │  ├─ Data/
│     │  │  ├─ Postgres/
│     │  │  │  ├─ AuthDbContext.cs              # DbContext com modelos OpenIddict
│     │  │  │  └─ Seeds/
│     │  │  │     └─ OpenIddictSeedService.cs   # Seeding de clientes e escopos
│     │  ├─ Extensions/
│     │  │  └─ DatabaseDependencyExtensions.cs  # Configuração do EF Core
│     │  ├─ Program.cs                          # Configuração da aplicação e OpenIddict
│     │  ├─ appsettings.json                    # Configurações padrão
│     │  ├─ appsettings.Development.json        # Configurações de desenvolvimento
│     │  └─ FoodChallenge.Auth.Api.csproj
│     └─ FoodChallenge.Auth.sln                 # Solução .NET
│
├─ tools/
│  ├─ docker/                                   # Arquivos Docker e docker-compose
│  │  ├─ docker-compose.yml                     # Orquestração de containers
│  │  ├─ docker-compose-k8s.yml                 # Configuração para Kubernetes
│  │  ├─ .env                                   # Variáveis de ambiente
│  │  ├─ build/                                 # Dockerfiles da aplicação
│  │  └─ init-db/                               # Scripts de inicialização do banco
│  │
│  └─ postman/                                  # Collections e Environment para Postman
│
├─ .gitignore                                   # Configurações de ignore do git
├─ README.md                                    # Conteúdo deste documento
```

---

## 🚀 Tecnologias Utilizadas

- [.NET 9](https://dotnet.microsoft.com/download)
- **Entity Framework Core 9**
- **PostgreSQL 12+**
- **Docker / Docker Compose**
- **OpenIddict 7.2.0** (OAuth2/OpenID Connect)
- **Npgsql.EntityFrameworkCore.PostgreSQL 9.0.2**
- **Clean Architecture**

---

## ▶️ Como Executar

### ✅ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- Git

---

### Migrações do Entity Framework

As migrações do Entity Framework são executadas automaticamente quando a aplicação inicia via Docker. Porém, se precisar executar manualmente ou criar novas migrações, utilize os comandos abaixo:

#### Aplicar Migrações
```bash
# Via dotnet CLI (no diretório do projeto)
cd src/FoodChallenge.Auth/FoodChallenge.Auth.Api
dotnet ef database update --project ../FoodChallenge.Auth.Api --startup-project .
```

#### Criar Nova Migração
```bash
cd src/FoodChallenge.Auth/FoodChallenge.Auth.Api
dotnet ef migrations add NomeDaMigracao --project ../FoodChallenge.Auth.Api --output-dir Data/Postgres/Migrations
```

#### Remover Última Migração
```bash
cd src/FoodChallenge.Auth/FoodChallenge.Auth.Api
dotnet ef migrations remove --project ../FoodChallenge.Auth.Api
```

> 📝 **Nota**: As migrações devem ser criadas no projeto `FoodChallenge.Auth.Api` e aplicadas através do projeto `FoodChallenge.Auth.Api` que contém a configuração de startup.

---

### 🐳 Subindo com Docker

#### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/fiap-tech-challenge-auth.git
cd fiap-tech-challenge-auth
```

#### 2. Gerar Certificado HTTPS de Desenvolvimento

A API requer HTTPS para funcionar corretamente. Antes de iniciar os containers, gere um certificado de desenvolvimento:

**Windows (PowerShell):**
```powershell
# Criar diretório para certificados
$certPath = Join-Path $env:USERPROFILE ".aspnet\https"
New-Item -ItemType Directory -Force -Path $certPath

# Gerar certificado
dotnet dev-certs https -ep "$certPath\aspnetapp.pfx" -p "DevCert@2024"

# Confiar no certificado (opcional, mas recomendado)
dotnet dev-certs https --trust
```

**Linux/macOS:**
```bash
# Criar diretório para certificados
mkdir -p ~/.aspnet/https

# Gerar certificado
dotnet dev-certs https -ep ~/.aspnet/https/aspnetapp.pfx -p "DevCert@2024"

# Confiar no certificado (opcional)
dotnet dev-certs https --trust
```

**Configurar senha no arquivo .env:**

Adicione a variável `CERT_PASSWORD` no arquivo `tools/docker/.env`:
```env
CERT_PASSWORD=DevCert@2024
```

> 📝 **Nota**: Use a mesma senha definida no comando de geração do certificado.

#### 3. Iniciar o banco de dados com Docker

```bash
cd tools/docker
docker-compose up -d foodchallenge_db
```

Aguarde até que o PostgreSQL esteja pronto (cerca de 5-10 segundos).

#### 4. Restaurar dependências e executar a API

```bash
cd ../../src/FoodChallenge.Auth
dotnet restore

cd FoodChallenge.Auth.Api
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

> ⚠️ **Importante**: Por padrão, o OpenIddict **requer HTTPS**. Certifique-se de que o certificado foi gerado corretamente conforme o passo 2.

#### 5. Verificar se a aplicação está funcionando

```bash
# Obter um token de acesso
curl -X POST https://localhost:5001/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=orders-api&client_secret=orders-secret-key-123&grant_type=client_credentials&scope=orders.read%20orders.write" \
  --insecure
```

Você deve receber uma resposta JSON com o `access_token`.

---

### 🔗 APIs Disponíveis

Após subir a aplicação, acesse a documentação interativa:

👉 [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)

Ou importe a collection Postman localizada em:

📁 `tools/postman/FoodChallenge.Auth.postman_collection.json`

## 📡 Endpoints

### 1. Obter Token (OAuth2 Client Credentials)

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

client_id=orders-api&
client_secret=orders-secret-key-123&
grant_type=client_credentials&
scope=orders.read%20orders.write
```

**Resposta (200)**:
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer",
  "expires_in": 3600,
  "scope": "orders.read orders.write"
}
```

### 2. Validar Token (Introspection)

```http
POST /connect/introspect
Authorization: Basic b3JkZXJzLWFwaTpvcmRlcnMtc2VjcmV0LWtleS0xMjM=
Content-Type: application/x-www-form-urlencoded

token=<ACCESS_TOKEN>
```

**Resposta (200)**:
```json
{
  "active": true,
  "scope": "orders.read orders.write",
  "client_id": "orders-api",
  "token_type": "Bearer",
  "exp": 1705000000
}
```

## � Endpoints Disponíveis

### 1. Obter Token (POST /connect/token)

Obtém um token de acesso usando o fluxo Client Credentials.

**Request:**
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

client_id=orders-api&
client_secret=orders-secret-key-123&
grant_type=client_credentials&
scope=orders.read%20orders.write
```

**Response (200 OK):**
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer",
  "expires_in": 3600,
  "scope": "orders.read orders.write"
}
```

### 2. Validar Token (POST /connect/introspect)

Valida e obtém informações sobre um token.

**Request:**
```http
POST /connect/introspect
Content-Type: application/x-www-form-urlencoded
Authorization: Basic b3JkZXJzLWFwaTpvcmRlcnMtc2VjcmV0LWtleS0xMjM=

token=<ACCESS_TOKEN>
```

**Response (200 OK):**
```json
{
  "active": true,
  "scope": "orders.read orders.write",
  "client_id": "orders-api",
  "token_type": "Bearer",
  "exp": 1705000000
}
```

## 🔐 Segurança

### Client Secrets

Os clientes (aplicações) registrados no sistema de autenticação podem se autenticar usando:
- `client_id` - Identificador único da aplicação
- `client_secret` - Chave secreta (usada apenas em desenvolvimento/staging)
- Fluxo **Client Credentials** para serviço-a-serviço

### Recomendações para Produção

- ✅ Use variáveis de ambiente para `client_secret` (não commit no repositório)
- ✅ Implemente rotação de secrets regularmente
- ✅ Use HTTPS obrigatório
- ✅ Valide a origem dos requests
- ✅ Implemente rate limiting nos endpoints de token
- ✅ Monitore acessos suspeitos
- ✅ Hash ou criptografe secrets no banco de dados

---

## 🗄️ Modelo de Dados

O projeto utiliza **Entity Framework Core** com **PostgreSQL**. O banco de dados é criado automaticamente com as seguintes tabelas do **OpenIddict**:

### Tabelas Gerenciadas pelo OpenIddict

- **openiddict_applications** - Clientes registrados (ex: orders-api, kitchen-api)
- **openiddict_authorizations** - Autorizações concedidas aos clientes
- **openiddict_scopes** - Escopos disponíveis no sistema
- **openiddict_tokens** - Tokens de acesso emitidos

### Dados Seeding (Inicialização)

Na primeira execução da aplicação, o serviço `IOpenIddictSeedService` registra automaticamente:

**Cliente: orders-api**
- Client ID: `orders-api`
- Client Secret: `orders-secret-key-123`
- Scopes: `orders.read`, `orders.write`

**Escopos Padrão:**
- `orders.read`, `orders.write`
- `configuration.read`, `configuration.write`

---

## 🔧 Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=foodchallenge_auth;Username=auth_user;Password=Auth@2024Secure#Pass"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=foodchallenge_auth;Username=auth_user;Password=Auth@2024Secure#Pass"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Variáveis de Ambiente

Você pode sobrescrever as configurações usando variáveis de ambiente:

```bash
# Connection String
export CONNECTIONSTRINGS_POSTGRESQL="Host=localhost;Port=5432;Database=foodchallenge_auth;Username=auth_user;Password=Auth@2024Secure#Pass"

# Ambiente
export ASPNETCORE_ENVIRONMENT=Development

# URLs de escuta
export ASPNETCORE_URLS=https://+:5001;http://+:5000
```

---

## 📦 Dependências Principais

**Pacotes NuGet instalados:**

- `OpenIddict.AspNetCore` (7.2.0) - Implementação de OAuth2/OpenID Connect
- `OpenIddict.EntityFrameworkCore` (7.2.0) - Integração com EF Core
- `Npgsql.EntityFrameworkCore.PostgreSQL` (9.0.2) - Driver PostgreSQL para EF Core
- `Microsoft.EntityFrameworkCore.Design` (9.0.0) - Ferramentas de design do EF Core (CLI)

**Runtime:**

- .NET 9.0 SDK
- PostgreSQL 12+

**Ferramentas CLI:**

Para executar migrations via linha de comando:

```bash
# Instalar globalmente (se necessário)
dotnet tool install --global dotnet-ef

# Ou dentro do projeto
dotnet tool install dotnet-ef
```

## 🧪 Testes da API

### Testar com curl

#### 1. Obter Token de Acesso

```bash
# Cliente: orders-api
TOKEN=$(curl -X POST https://localhost:5001/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=orders-api&client_secret=orders-secret-key-123&grant_type=client_credentials&scope=orders.read%20orders.write" \
  --insecure | jq -r '.access_token')

echo $TOKEN
```

#### 2. Validar Token (Introspection)

```bash
TOKEN="seu-token-aqui"

curl -X POST https://localhost:5001/connect/introspect \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -H "Authorization: Basic b3JkZXJzLWFwaTpvcmRlcnMtc2VjcmV0LWtleS0xMjM=" \
  -d "token=$TOKEN" \
  --insecure | jq
```

### Testar com arquivo .http

O projeto inclui um arquivo `FoodChallenge.Auth.Api.http` com exemplos prontos para usar no VS Code com a extensão REST Client.

### Testar com PowerShell

```powershell
# Obter token
$response = Invoke-WebRequest -Uri "https://localhost:5001/connect/token" `
    -Method Post `
    -Body @{
        client_id = "orders-api"
        client_secret = "orders-secret-key-123"
        grant_type = "client_credentials"
        scope = "orders.read orders.write"
    } `
    -SkipCertificateCheck

$token = ($response.Content | ConvertFrom-Json).access_token
echo $token

# Validar token
$headers = @{
    "Authorization" = "Basic $(
        [Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes('orders-api:orders-secret-key-123'))
    )"
}

Invoke-WebRequest -Uri "https://localhost:5001/connect/introspect" `
    -Method Post `
    -Body @{ token = $token } `
    -Headers $headers `
    -SkipCertificateCheck | Select-Object Content
```

Para exemplos mais detalhados, veja o arquivo [TEST_EXAMPLES.md](TEST_EXAMPLES.md).

## 📖 Documentação Adicional

- [Project Summary](PROJECT_SUMMARY.md) - Resumo geral do projeto
- [API Guide](API_GUIDE.md) - Documentação detalhada de endpoints e fluxos
- [Test Examples](TEST_EXAMPLES.md) - Exemplos de testes da API
- [Deployment Guide](DEPLOYMENT.md) - Instruções para deploy em produção
- [OpenIddict Docs](https://documentation.openiddict.com/) - Documentação oficial do OpenIddict
- [OAuth 2.0 RFC 6749](https://tools.ietf.org/html/rfc6749) - Especificação OAuth 2.0
- [OpenID Connect](https://openid.net/specs/openid-connect-core-1_0.html) - Especificação OpenID Connect

## 🤝 Contribuindo

1. Fork o repositório
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Add MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a MIT License - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👨‍💻 Autor

Desenvolvido para o FIAP Tech Challenge Auth

## 📞 Suporte

Para dúvidas ou problemas:
1. Verifique a [documentação](API_GUIDE.md)
2. Consulte os [exemplos de teste](TEST_EXAMPLES.md)
3. Abra uma issue no repositório
