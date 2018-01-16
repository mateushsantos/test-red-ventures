# Red Ventures Backend Test

Web API using ASP.NET Core, Sql Server and Docker

## Getting Started

These instructions will get you a copy of the project up and running on your local machine.

### Prerequisites

```
Docker
```

### Installing

First of all, let's generate a local docker image from our application, from the root path

```
docker build -t rv-test -f RV.Test/Dockerfile .
```

After built, we need to be sure that the docker engine has at least 4gb or ram allocated for SQL Server,

Then use docker-compose to run everything

```
docker-compose up -d
```

After that, the application will be running at localhost:8080, the SQL Server will be at localhost:1401, and swagger will be in localhost:8080/swagger

The containers names are rvtestweb and rvtestdb, to stop it just run

```
docker stop <container-name>
```

And then to remove it

```
docker rm <container-name>
```

## Swagger and Ideas

To use a Token to authenticate, I've created two routes, /admin/signup and /admin/authenticate

So you first need to Sign Up with an Admin and then Authenticate, it'll return a JWT token, and that's what you'll need to see the other routes and to Authorize in Swagger using Bearer {token}

Also, added a route to POST users, I thought it'd make sense, since users route have 2 GET's

## Built With

* [Docker](https://docs.docker.com/) - The containerization platform
* [Swagger](https://swagger.io/docs/) - API Tooling 
* [ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/) - Web Framework
* [Sql Server](https://docs.microsoft.com/pt-br/sql/linux/quickstart-install-connect-docker) - SQL Database