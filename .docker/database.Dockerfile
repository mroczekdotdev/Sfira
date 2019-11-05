FROM postgres:11-alpine

COPY postgresql-initial.sql /docker-entrypoint-initdb.d/000-init.sql
