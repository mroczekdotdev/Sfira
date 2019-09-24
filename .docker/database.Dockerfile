FROM postgres:11-alpine

COPY postgres.sql /docker-entrypoint-initdb.d/0-init.sql
