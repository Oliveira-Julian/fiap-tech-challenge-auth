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

A estrutura implementa os padrões **OAuth2** e **OpenID Connect** através da biblioteca **OpenIddict**, permitindo que outras aplicações e microserviços se autentiquem de forma segura e consistente.

### Recursos Principais

- **OAuth2 + OpenID Connect** com OpenIddict 7.2.0
- **Fluxo Client Credentials** para comunicação serviço-a-serviço
- **Endpoints OAuth2**:
  - `/connect/token` - Obter tokens de acesso
  - `/connect/introspect` - Validar e inspecionar tokens
- **Entity Framework Core 9** com PostgreSQL 17
- **Seeding automático** de dados iniciais (clientes e escopos)

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
├─ tools/
│  ├─ docker/                                   # Arquivos Docker e docker-compose
│  │  ├─ docker-compose.yml                     # Orquestração de containers
│  │  ├─ docker-compose-k8s.yml                 # Configuração para Kubernetes
│  │  ├─ .env                                   # Variáveis de ambiente
│  │  ├─ build/                                 # Dockerfiles da aplicação
│  │  └─ init-db/                               # Scripts de inicialização do banco
│  └─ postman/                                  # Collections e Environment para Postman
│
├─ .gitignore                                   # Configurações de ignore do git
├─ README.md                                    # Conteúdo deste documento
```

---

## 🚀 Tecnologias Utilizadas

- [.NET 9](https://dotnet.microsoft.com/download)
- **Entity Framework Core 9**
- **PostgreSQL**
- **Docker / Docker Compose**
- **OpenIddict 7.2.0** (OAuth2/OpenID Connect)

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

---

### 🐳 Subindo com Docker

#### 1. Gerar Certificado HTTPS de Desenvolvimento

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

#### 2. Subir os containers

```bash
cd tools/docker
docker-compose up -d --build
```

Esse comando irá subir os seguintes serviços:

- **foodchallenge_postgres_db**: banco de dados PostgreSQL
- **foodchallenge_auth_migrations**: aplicação das migrations de autenticação
- **foodchallenge_auth_api**: aplicação Web API de autenticação (.NET 9)

> ⚠️ **Importante**: Por padrão, o OpenIddict **requer HTTPS**. Certifique-se de que o certificado foi gerado corretamente conforme o passo 1.

### 🔗 APIs Disponíveis

Após subir os containers, importe a collection Postman localizada em:

📁 `tools/postman/Fiap - Tech Challenge - Auth.postman_collection.json`