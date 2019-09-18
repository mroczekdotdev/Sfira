FROM postgres:11

COPY postgres.sql /docker-entrypoint-initdb.d/0-init.sql
