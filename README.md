## Technologies
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [Mapster](https://github.com/MapsterMapper/Mapster)
* [FluentValidation](https://fluentvalidation.net/)
* [Elasticsearch](https://www.elastic.co/), [Serilog](https://serilog.net/), [Kibana](https://www.elastic.co/kibana)
* [Docker](https://www.docker.com/)

## Getting Started

1. Pull the Elastic Container image, to pull run `docker pull docker.elastic.co/elasticsearch/elasticsearch:8.12.0`.

2. Run the container with command `docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "xpack.security.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:8.12.0`.

3. Verify elastic container is running, goto chrome search bar and try visiting "localhost:9200/_aliases".

4. Edit the app.settings.json file inside the project AuthApp.Infrastructure.Api.
5. Run the project migrations for sql server, open the package manager console in visual studio and run `dotnet ef migrations add "CreateDb" --project src\Common\AuthApp.Infrastructure.SqlServer\AuthApp.Infrastructure.SqlServer.csproj --startup-project src\Apps\AuthApp.Api.csproj`. You should see Migrations folder inside project AuthApp.Infrastructure.SqlServer
6. Run the project using IISExpress profile. Project will take some time, as it will take some time seeding the initial application data.
8. Download the postman collection from root of the project to test the apis.


## Authentication apis details:
1. `api/auth/login` Api accepts json body and structure of body is below:
Action: Post
`{
  "username": "azharriaz",
  "password": "Azhar@321"
}`

2. `api/auth/refresh-token` Api accepts json body with structure:
Action: Post
`{
  "accessToken": "{{jwtToken}}",
  "refreshToken": "{{refreshToken}}"
}`

## Users apis details:
1. `api/users` Api accepts json body and structure of body is below:
Action: Post
`{
  "username": "JohnDoe",
  "email": "john@example.com",
  "password": "Azhar@321",
  "firstname":"John",
  "lastname": "Doe",
  "roles": ["Manager"]
}`

2. `api/users` Api accepts json body and structure of body is below:
Action: Put
`{
  "userid": "7a1da7e8-4177-411c-b923-e9960851cc39",
  "name": "John D Updated",
  "email": "john.doe@example.com",
  "roles": ["Administrator"]
}`

3. `api/users/{userId}` Api accepts query param userId (sample: '7a1da7e8-4177-411c-b923-e9960851cc39').
Action: Get

3. `api/users/{userId}` Api accepts query param userId (sample: '7a1da7e8-4177-411c-b923-e9960851cc39').
Action: Delete

## Roles apis details:
1. `api/roles` Api accepts json body and structure of body is below:
Action: Post
`{
  "name": "Manager"
}`

2. `api/roles` Api accepts json body and structure of body is below:
Action: Put
`{
  "id": "27c22eeb-36be-4b0e-bf04-59392fd64eec",
  "name": "Manager Updated"
}`

3. `api/roles/{roleId}` Api accepts query param roleId (sample: '27c22eeb-36be-4b0e-bf04-59392fd64eec').
Action: Get

3. `api/roles/{roleId}` Api accepts query param roleId (sample: '27c22eeb-36be-4b0e-bf04-59392fd64eec').
Action: Delete

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **WebApi/appsettings.json** as follows:

```json
  "DbProvider": SqlServer
```

### Multiple databases migrations
To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project src/Common/AuthApp.Infrastructure.{DbProvider}`
* `--startup-project src/Apps/AuthApp.Api`

For example, to add a new migration from the root folder:

set `"DbProvider"` in **appsettings.json** of Api project to `SqlServer`:
`dotnet ef migrations add "CreateDb" --project src\Common\AuthApp.Infrastructure.SqlServe --startup-project src\Apps\AuthApp.Api`

`dotnet ef database update --project src\Common\AuthApp.Infrastructure.SqlServer --startup-project src\Apps\WebApi`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebApi

This layer is a web api application based on ASP.NET 9.0.x. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

### Logs

Logging into Elasticsearch using Serilog and viewing logs in Kibana.


