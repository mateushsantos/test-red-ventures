# Red Ventures Backend Test

Web API using ASP.NET Core, Sql Server and Docker

## Getting Started

These instructions will get you a copy of the project up and running on your local machine.

### Prerequisites

```
Docker
```

### Installing

First of all, let's generate a local docker image from our application

```
docker build -t rv-test -f RV.Test/Dockerfile .
```

After built, we need to use docker-compose to run it

```
docker-compose up -d
```

After that, the application will be running at localhost:8080, and swagger will be in localhost:8080/swagger

## Built With

* [Docker](https://docs.docker.com/) - The containerization platform
* [Swagger](https://swagger.io/docs/) - API Tooling 
* [ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/) - Web Framework
* [Sql Server](https://docs.microsoft.com/pt-br/sql/linux/quickstart-install-connect-docker) - SQL Database