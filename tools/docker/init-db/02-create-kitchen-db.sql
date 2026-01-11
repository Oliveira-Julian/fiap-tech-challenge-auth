-- Script de inicialização do PostgreSQL
-- Cria banco de dados 'foodchallenge_kitchen' com usuário 'kitchen_user'

-- Criar o usuário 'kitchen_user' com uma senha segura
CREATE USER kitchen_user WITH PASSWORD 'Kitchen@2024Secure#Pass';

-- Criar o banco de dados 'foodchallenge_kitchen'
CREATE DATABASE foodchallenge_kitchen OWNER kitchen_user;

-- Conectar ao banco de dados foodchallenge_kitchen
\c foodchallenge_kitchen;

-- Dar permissões ao usuário kitchen_user no banco foodchallenge_kitchen
GRANT CONNECT ON DATABASE foodchallenge_kitchen TO kitchen_user;
GRANT USAGE ON SCHEMA public TO kitchen_user;
GRANT CREATE ON SCHEMA public TO kitchen_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO kitchen_user;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO kitchen_user;

-- Garantir que futuros objetos criados também sejam acessíveis
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO kitchen_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE ON SEQUENCES TO kitchen_user;
