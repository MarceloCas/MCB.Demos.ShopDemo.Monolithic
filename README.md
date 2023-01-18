
# MCB.Demos.ShopDemo.Monolithic

## :heavy_exclamation_mark: THIS PROJECT IS UNDER CONSTRUCTION :heavy_exclamation_mark:

This project is a source code of MCB.Demos.ShopDemo.Monolithic used during the mentorships carried out by me. You can find my profile on Linkedin (https://www.linkedin.com/in/marcelocastelobranco/).

This project has some applied concepts like Domain-Driven Design, Event-Driven, CQRS, Distributed Cache, Observability, Devops and more.

:information_source: Apparently this project has over-engineering, but aspects such as the business context, size and qualification of the team, volume of accesses, availability, elasticity, etc. are addressed during mentoring and contextualized there.

:warning: This project is not recommended under any circumstances to be used as a template or model of any project.

:stop_sign: I am not responsible for the use of any item from this project in any other project than this one.

:books: This project is to be used for study purposes only. I hope this project helps you in your studies!

## :white_check_mark: Labels

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=coverage)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)


[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)


[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=MarceloCas_MCB.Demos.ShopDemo.Monolithic&metric=bugs)](https://sonarcloud.io/summary/new_code?id=MarceloCas_MCB.Demos.ShopDemo.Monolithic)

## :book: Table of contents
* [Prerequisite](#prerequisite)
* [Key features](#key-features)
* [Technologies](#technologies)
* [Architecture](#architecture)
* [Design Patterns](#design-patterns)
* [Dependencies](#dependencies)
* [Setup](#setup)
* [Authors](#authors)

## :warning: Prerequisite
To setup and run this project you will need:
- .NET 7 SDK installed
- Basic .NET and C# skills
- Docker installed and running if you dont have all dependencies installed
- Docker compose knowledge if you dont have all dependencies installed

## :star: Key features
This project has amazing features, but the key features that we don't find in most projects are:
- Multitenant
- Devops (CI/CD with build, integration tests and docker image publish)
- Observability with trace, metrics and dashboards
- Dependency injection
- Resilience
- Notifications
- Heath checks
- API pagination and filtering
- HATEOAS
- Transactions
- Batch operations
- Async operations
- Static code analysis
- Integration tests
- Regression tests
- Load tests
- Chaos testing
- Docker image build and publish

## :computer: Technologies
This project uses amazing technologies and frameworks like:
- .NET 7 as application plataform
- C# as programming language
- ASP.NET Identity with JWT
- ASP.NET Core Web Api for BFF API and REST API
- ASP.NET Core GRPC Services for BFF and Web API
- Worker Services for message queue consumers
- .NET Framework Core as ORM framework to relational database access and migrations
- Polly as resilience framework
- FluentValidation for validations
- PostgreSQL as relational database
- Redis as no-sql key/value database for distributed cache purpose
- RabbitMQ as message queue
- OpenTelemetry SDK as observability framework
- Jaeger as trace viewer software
- Prometheus as metrics storage
- Grafana as dashbaord software
- Github Actions as CI/CD platform

## :classical_building: Architecture
This project uses some key concepts about software architecture:
- Multitenant
- Domain-Driven Design for domain modeling (Although DDD is not about architecture but about domain modeling, I decided to put it in this topic)
- Rich domain models
- Domain Specifications
- Domain Events
- Data Models
- Use Case Approach
- CQRS (Command Query Responsibility Segregation)
- Health check probes (startup, readiness and liveness)
- High availability using resiliency techniques
- Observability features as trace, metrics and logs
- Distributed caching

## :notebook: Design Patterns
Design patterns are essential for good maintainable code. This project uses some object-oriented programming principles and design patterns:
- SOLID principles
- Singleton pattern
- Factory pattern
- Adapter pattern
- Specification pattern
- Command pattern
- Repository pattern
- Facade pattern
- Strategy pattern
- Retry pattern
- Circuit Breaker pattern
- Unit of Work pattern
- Observer pattern

## :chains: Dependencies

If you don't have a docker installed and running, you will need to have the following systems installed and running to execute this project:
- Postgres
- Redis
- Prometheus
- RabbitMQ
- Jaeger
- Grafana


## :gear: Setup

If you dont have the dependencies do run this project, you can setup then using docker. This project have this [docker-compose.yml](docker-compose.yml) file to setup all dependencies.

This compose file will create:
- Docker network called mcb-demos-shopdemo-monolithic using brigde drive
- Jaeger service
- RabbitMQ service with management interface
- PostgreSQL database instance
- PgAdmin installation to access PostgreSQL dabase using a internet browser
- MongoDB database single instance
- Mongo Express installation to acess MongoDB using a internet browser
- Redis single instance
- Redis Insight installation to access Redis using a internet browser
- Portainer to view and manage all compose encironment using a internet browser GUI

:information_source: All username, passwords and ports mapping are in  [docker-compose.yml](docker-compose.yml)  file :information_source:

### Up docker compose file directly to the host

To up the compose file directly to the host that has docker running, go in a terminal to the root directory of that repository that contains the docker-compose.yml file and run the command:

```bash
cd /mcb/github/marcelocas/demos.shopdemo.monolithic/
docker compose up -d
```


### Up docker compose file using WSL2

To up the compose file in a docker installation inside a Windows OS using WSL2 you can use a [setup-wsl.ps1](setup-wsl.ps1) powershell script file using the Powershell terminal.

:warning: This script will shutdown your WSL2

:warning: This script will up docker compose file in default WSL2 distro

:warning: This script area created to run in a Ubuntu image

```powershell
cd C:\mcb\github\marcelocas\Demos.ShopDemo.Monolithic
.\setup-wsl.ps1
```


## :people_holding_hands: Authors

- [@MarceloCas](https://www.linkedin.com/in/marcelocastelobranco/)


