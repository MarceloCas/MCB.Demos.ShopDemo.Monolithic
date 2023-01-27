
# MCB.Demos.ShopDemo.Monolithic

## :heavy_exclamation_mark: THIS PROJECT IS UNDER CONSTRUCTION :heavy_exclamation_mark:

This project is a source code of MCB.Demos.ShopDemo.Monolithic used during the mentorships carried out by me. You can find my profile on Linkedin (https://www.linkedin.com/in/marcelocastelobranco/).

In future mentorships, I will migrate this project to distributed software using a microservices approach with migration strategies from monolithic applications to distributed applications using microservices.

I decided to start the project with a monolithic application because it's crazy to create microservices without knowing how to build resilient applications, with observability, with asynchronous processing and knowledge in event-driven applications. So nothing better than applying this knowledge in a monolithic application, after all, if we cannot apply these concepts in a single monolithic application, we will never be able to make several independent applications that integrate with each other and have things like eventual consistency as microservices require.

This project has some applied concepts like Domain-Driven Design, Event-Driven, CQRS, Distributed Cache, Observability, Devops and more.

:information_source: Apparently this project has over-engineering, but aspects such as the business context, size and qualification of the team, volume of accesses, availability, elasticity, etc. are addressed during mentoring and contextualized there.

:warning: This project is not recommended under any circumstances to be used as a template or model of any project.

:stop_sign: I am not responsible for the use of any item from this project in any other project than this one.

:books: This project is to be used for study purposes only. I hope this project helps you in your studies!

## :white_check_mark: Labels

[![ci](https://github.com/MarceloCas/MCB.Demos.ShopDemo.Monolithic/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/MarceloCas/MCB.Demos.ShopDemo.Monolithic/actions/workflows/ci.yml)
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
* [Run](#run)
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
- Feature Flags
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
- HashiCorp Consul KV
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
- PostgreSQL
- Redis
- Prometheus
- RabbitMQ
- Jaeger
- Grafana
- HashiCorp


## :gear: Setup

If you dont have the dependencies do run this project, you can setup then using docker. This project have this [docker-compose.yml](docker-compose.yml) file to setup all dependencies.

This compose file will create:
- Docker network called mcb-demos-shopdemo-monolithic using brigde drive
- [Jaeger service](http://localhost:16686/search)
- [RabbitMQ](http://localhost:15672/) service with management interface
- PostgreSQL database instance
- [PgAdmin](http://localhost:8080/browser/) installation to access PostgreSQL dabase using a internet browser
- MongoDB database single instance
- [Mongo Express](http://localhost:8081/) installation to acess MongoDB using a internet browser
- Redis single instance
- [Redis Insight](http://localhost:8001/) installation to access Redis using a internet browser
- [HashiCorp Consult KV](http://localhost:8500/) installation to access HashiCorp KV
- [Portainer](http://localhost:9000/) to view and manage all compose environment using a internet browser GUI

:information_source: All username, passwords and ports mapping are in  [docker-compose.yml](docker-compose.yml)  file :information_source:

### :arrow_right:  Up docker compose file directly to the host

To up the compose file directly to the host that has docker running, go in a terminal to the root directory of that repository that contains the docker-compose.yml file and run the command:

```bash
cd /mcb/github/marcelocas/demos.shopdemo.monolithic/
docker compose up -d
```


### :arrow_right: Up docker compose file using WSL2

To up the compose file in a docker installation inside a Windows OS using WSL2 you can use a [setup-wsl.ps1](setup-wsl.ps1) powershell script file using the Powershell terminal.

:warning: This script will shutdown your WSL2 :warning:

:warning: This script will up docker compose file in default WSL2 distro :warning:

:warning: This script area created to run in a Ubuntu image :warning:

```powershell
cd C:\mcb\github\marcelocas\Demos.ShopDemo.Monolithic
.\setup-wsl.ps1
```

### :arrow_right: Portainer ###

After up docker compose file you must have initialize [Portainer](http://localhost:9000/) service. If you take time to access the portainer, you will have to restart the portainer docker service and try to access again using a command in terminal from repository root path like:

Linux or Windows Docker Desktop:
```bash
docker compose restart portainer
```

Windows with docker installed in WSL:
```powershell
wsl -e docker compose restart portainer
```

### :arrow_right: Feature Flags ###

:information_source: All resource flags are imported as active for the tenant **{0275a76e-0b7d-4187-b10e-5ef540e266e9}** :information_source:

The feature flags are in [consul-kv-import.json](consul-kv-import.json) file. This file have all feature flag keys with value "true" encoded as based 64 string:

|text|base64
|---|---
|true|dHJ1ZQ==|  
|false|ZmFsc2U=|  

Example:

```json
[
  {
    "Key": "feature-flags/tenants/0275a76e-0b7d-4187-b10e-5ef540e266e9/import-customer",
    "Value": "dHJ1ZQ=="
  },
  {
    "Key": "feature-flags/tenants/0275a76e-0b7d-4187-b10e-5ef540e266e9/import-customer-batch",
    "Value": "dHJ1ZQ=="
  }
]
```

:warning: if the feature flags are not imported (you can see in [HashiCorp Consult KV](http://localhost:8500/)), you can access [Portainer](http://localhost:9000/) and view logs and try start consul-import service again :warning:

### :arrow_right: Apply migrations on database

:warning: You need to have an instance of PostgreSQL running with credentials that have permission to run DDL commands.
If you created the project's dependencies using the docker compose provided in the project, you will already have that user and you can use the following command to apply the migration :warning:

In a terminal, from root repository path, run:

Linux:
```bash
dotnet tool install --global dotnet-ef
cd src/MCB.Demos.ShopDemo.Monolithic.Infra.Data
dotnet ef database update -- "Host=localhost;Port=5432;Username=admin;Password=123456;Database=mcb_demos_shopdemo_monolithic"
```

Windows powershell terminal:
```powershell
dotnet tool install --global dotnet-ef
cd .\src\MCB.Demos.ShopDemo.Monolithic.Infra.Data\
dotnet ef database update -- "Host=localhost;Port=5432;Username=admin;Password=123456;Database=mcb_demos_shopdemo_monolithic"
```

the output is simillar to:
```text
Build started...
Build succeeded.
Applying migration '20230111034438_InitialCreation'.
Done.
```


### :arrow_right: Define settings on AppSettings.json

The [AppSettings.json](src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi/appsettings.json) file is under in MCB.Demos.ShopDemo.Monolithic.Services.WebApi project.

For security reasons, credentials should never be exposed in the repository. That's why project settings are defined locally through [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows).
Regardless of whether you want to use user secrets or directly change the [AppSettings.json](src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi/appsettings.json) file, the following is an example configuration that will work if you created the environment using the project's docker-compose.yml

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379,allowAdmin=true,abortConnect=false,connectTimeout=500,responseTimeout=500,syncTimeout=500,keepAlive=10",
    "TTLSeconds": {
      "CustomerDataModel": 600
    }
  },
  "PostgreSql": {
    "ConnectionString": "Host=localhost;Port=5432;Username=admin;Password=123456;Database=mcb_demos_shopdemo_monolithic"
  },
  "RabbitMq": {
    "Connection": {
      "ClientProvidedName": "MCB.Demos.ShopDemo.Monolithic.Services.WebApi",
      "HostName": "localhost",
      "Port": 5672,
      "Username": "guest",
      "Password": "guest",
      "VirtualHost": "/",
      "DispatchConsumersAsync": true,
      "AutoConnect": true,
      "AutomaticRecoveryEnabled": true,
      "NetworkRecoveryIntervalSeconds": 5,
      "TopologyRecoveryEnabled": true,
      "RequestedHeartbeatSeconds": 60
    },
    "EventsExchange": {
      "Name": "mcb.demos.shopdemo.monolithic.e.events",
      "Durable": false,
      "AutoDelete": false,
      "Arguments": []
    }
  },
  "OpenTelemetry": {
    "GrpcCollectorReceiverUrl": "http://localhost:4317",
    "EnableConsoleExporter": true
  },
  "Consul": {
    "Address": "http://localhost:8500/v1/kv",
    "RefreshIntervalInSeconds": 0
  }
}
```

## :rocket: Run

:warning: All setup instructions must have been followed, especially the application of migrations and definition of configurations in the AppSettings.json file :warning:

:warning: You have all prerequisites and dependencies installed and running :warning:

In a terminal, from root repository path, run:

Linux:
```bash
cd src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi
dotnet run
```


Windows powershell terminal:
```powershell
cd .\src\MCB.Demos.ShopDemo.Monolithic.Services.WebApi\
dotnet run
```

the output is simillar to:
```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: YOUR_ROOT_DIRECTORY_PATH\src\MCB.Demos.ShopDemo.Monolithic.Services.WebApi
```

:warning: To change default application port for every execution you edit the [launchSettings.json](src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi/Properties/launchSettings.json) file, but you will need change in integration tests too. :warning:

### :arrow_right: Check if application is running correctly

We will use the Health Checks URL (https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/) to validate that the application is running correctly.

#### Validate Startup

Execute a HTTP GET operation in a `http://localhost:5000/health/startup` url. You can navigate to this URL in your internet browser.
The `StatusDescription` property value must be `Healthy`. The result is simmilar to:
```json
{
   "Date":"2023-01-18T01:51:42.4126434Z",
   "Status":1,
   "StatusDescription":"Healthy",
   "ServiceReportItemCollection":[]
}
```

#### Validate Readiness

Execute a HTTP GET operation in a `http://localhost:5000/health/readiness` url. You can navigate to this URL in your internet browser.
The `StatusDescription` property value must be `Healthy`. The result is simmilar to:
```json
{
   "Date":"2023-01-18T01:56:13.7142427Z",
   "Status":1,
   "StatusDescription":"Healthy",
   "ServiceReportItemCollection":[
      
   ]
}
```

#### Validate Liveness

Execute a HTTP GET operation in a `http://localhost:5000/health/liveness` url. You can navigate to this URL in your internet browser.
The `StatusDescription` property value must be `Healthy` for PostgreSQL, Redis and RabbitMq services. The result is simmilar to:
```json
{
   "Date":"2023-01-18T01:57:01.1350467Z",
   "Status":1,
   "StatusDescription":"Healthy",
   "ServiceReportItemCollection":[
      {
         "EntryName":"/health/liveness",
         "Status":1,
         "StatusDescription":"Healthy",
         "ServiceCollection":[
            {
               "Name":"PostgreSQL",
               "Status":1,
               "StatusDescription":"Healthy"
            },
            {
               "Name":"Redis",
               "Status":1,
               "StatusDescription":"Healthy"
            },
            {
               "Name":"RabbitMq",
               "Status":1,
               "StatusDescription":"Healthy"
            }
         ]
      }
   ]
}
```

After confirming that the application has started correctly, we can access the [swagger](http://localhost:5000/swagger/index.html) to use the available operations.

## :people_holding_hands: Authors

- [@MarceloCas](https://www.linkedin.com/in/marcelocastelobranco/)


