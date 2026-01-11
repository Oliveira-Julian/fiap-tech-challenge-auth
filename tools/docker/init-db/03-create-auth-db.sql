-- Script de inicialização do PostgreSQL
-- Cria banco de dados 'foodchallenge_auth' com usuário 'auth_user'

-- Criar o usuário 'auth_user' com uma senha segura
CREATE USER auth_user WITH PASSWORD 'Auth@2024Secure#Pass';

-- Criar o banco de dados 'foodchallenge_auth'
CREATE DATABASE foodchallenge_auth OWNER auth_user;

-- Conectar ao banco de dados foodchallenge_auth
\c foodchallenge_auth;

-- Dar permissões ao usuário auth_user no banco foodchallenge_auth
GRANT CONNECT ON DATABASE foodchallenge_auth TO auth_user;
GRANT USAGE ON SCHEMA public TO auth_user;
GRANT CREATE ON SCHEMA public TO auth_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO auth_user;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO auth_user;

-- Garantir que futuros objetos criados também sejam acessíveis
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO auth_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE ON SEQUENCES TO auth_user;
