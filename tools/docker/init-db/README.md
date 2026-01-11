# Database Initialization Scripts

Este diretório contém os scripts de inicialização do PostgreSQL para o projeto FoodChallenge Order.

## Arquivos

### `01-create-order-user.sql`
Script que cria o usuário de aplicação `order_user` com as permissões necessárias no banco de dados `foodchallenge_order`.

**Credenciais de acesso:**
- **Usuário:** `order_user`
- **Senha:** `Order@2024Secure#Pass`
- **Banco:** `foodchallenge_order`

### `02-create-kitchen-db.sql`
Script que cria o banco de dados `foodchallenge_kitchen` com o usuário `kitchen_user`.

**Credenciais de acesso:**
- **Usuário:** `kitchen_user`
- **Senha:** `Kitchen@2024Secure#Pass`
- **Banco:** `foodchallenge_kitchen`

### `03-create-auth-db.sql`
Script que cria o banco de dados `foodchallenge_auth` com o usuário `auth_user`.

**Credenciais de acesso:**
- **Usuário:** `auth_user`
- **Senha:** `Auth@2024Secure#Pass`
- **Banco:** `foodchallenge_auth`

## Execução

### Docker
Quando o container PostgreSQL é iniciado, todos os scripts SQL no formato `*.sql` neste diretório são automaticamente executados:

```bash
volumes:
  - ./init-db:/docker-entrypoint-initdb.d
```

O Docker inicia os scripts em ordem alfabética:
1. `01-create-order-user.sql` - Cria o usuário order_user no banco existente
2. `02-create-kitchen-db.sql` - Cria o banco kitchen e seu usuário
3. `03-create-auth-db.sql` - Cria o banco auth e seu usuário

### Kubernetes
No arquivo `postgres-init-sql-configmap.yaml`, o conteúdo deste script foi integrado no ConfigMap de inicialização.

## Segurança

Para ambientes de produção, é recomendado:

1. **Alterar a senha** do usuário `order_user` para uma senha mais robusta
2. **Usar secrets** do Kubernetes para armazenar credenciais
3. **Implementar rotação de senhas** regularmente
4. **Usar variáveis de ambiente** em vez de senhas hardcoded

## Strings de Conexão

### Order Service
```
Host=foodchallenge_db;Port=5432;Database=foodchallenge_order;Username=order_user;Password=Order@2024Secure#Pass
```

### Kitchen Service
```
Host=foodchallenge_db;Port=5432;Database=foodchallenge_kitchen;Username=kitchen_user;Password=Kitchen@2024Secure#Pass
```

### Auth Service
```
Host=foodchallenge_db;Port=5432;Database=foodchallenge_auth;Username=auth_user;Password=Auth@2024Secure#Pass
```

## Adicionando Novos Scripts

Para adicionar novos scripts de inicialização:

1. Crie um arquivo SQL com um prefixo numérico (ex: `02-criar-tabelas-adicionais.sql`)
2. Coloque o arquivo neste diretório
3. Reinicie o container PostgreSQL

Os scripts serão executados em ordem alfabética durante a inicialização do container.
