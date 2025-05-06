# baldurs_bank

# latest RabbitMQ 4.x
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management

# latest Postgres
docker run --name some-postgres -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -e POSTGRES_DB=abacaxi -d postgres

version: '3.8'

services:
  db:
    image: postgres
    container_name: postgres-multi-db
    restart: always
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: admin123
    ports:
      - "5432:5432"
    volumes:
      - ./init:/docker-entrypoint-initdb.d


```
CREATE DATABASE db1;
CREATE DATABASE db2;
CREATE DATABASE db3;
```