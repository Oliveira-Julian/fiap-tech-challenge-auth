-- Script de inicialização do PostgreSQL
-- Cria usuário 'order_user' com acesso ao banco 'foodchallenge_order'

-- Criar o usuário 'order_user' com uma senha segura
CREATE USER order_user WITH PASSWORD 'Order@2024Secure#Pass';

-- Criar o banco de dados 'foodchallenge_order'
CREATE DATABASE foodchallenge_order OWNER order_user;

-- Conectar ao banco de dados foodchallenge_order
\c foodchallenge_order;

-- Dar permissões ao usuário order_user no banco foodchallenge_order
GRANT CONNECT ON DATABASE foodchallenge_order TO order_user;
GRANT USAGE ON SCHEMA public TO order_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO order_user;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO order_user;

-- Garantir que futuros objetos criados também sejam acessíveis
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO order_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE ON SEQUENCES TO order_user;
