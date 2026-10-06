# Overview

The **RA.CleanArchitecture.Template** is a .NET solution template for ASP.NET Core that implements Clean Architecture principles. It gives you a **production-ready starting point** so you can focus on building features, not boilerplate.

## What is Clean Architecture?

Clean Architecture separates code into distinct layers with strict dependency rules, keeping business logic independent of frameworks, databases, and UI concerns. Business logic sits at the center of the application, and outer layers depend inward — never the other way around.

## What the template generates

Running `dotnet new RA.Template -n YourProjectName` scaffolds a complete multi-project solution:

| Project | Type | Description |
| :--- | :--- | :--- |
| `Domain` | Class Library | Encapsulates core business entities, constants, and rules — no dependencies on other layers |
| `Application` | Class Library | Defines use cases (CQRS handlers, validators, DTOs) and abstractions for infrastructure concerns |
| `Infrastructure` | Class Library | Central wiring for infrastructure components and dependency injection |
| `Integration` _(optional)_ | Class Library | Pre-configured HTTP client infrastructure with request/response logging; included when `UseIntegrations` is enabled |
| `Persistence` _(optional)_ | Class Library | Entity Framework Core data access — SQL Server, Oracle, PostgreSQL, or SQLite (one `DbContext` per selected provider); excluded when the database is `None` |
| `Api` | ASP.NET Core Web API | Exposes the API, handles routing, and (optionally) JWT authorization |
| `Api.Contracts` | Class Library | Shared request/response contracts with no project dependencies |
| `ArchitectureTests` | Test Project | NetArchTest-based tests enforcing layer boundaries and naming conventions |

## Key features

- **.NET 10** target framework
- **CQRS, Dependency Injection, and Mediator** patterns built in
- **JWT authorization** — include or exclude with a single parameter
- **OpenAPI documentation** via Scalar, served at `/openapi-ui`
- **Flexible persistence** — choose one, several, or no database providers
- **Ready-to-use setup** — preconfigured logging, validation, and exception handling
- **Architecture tests** — layer boundaries enforced from day one

## Who it's for

The template is designed for .NET developers who:

- Are starting a new enterprise application and want a solid foundation
- Want to apply Clean Architecture without spending days wiring up infrastructure
- Value testability, maintainability, and separation of concerns
- Are building REST APIs, microservices, or enterprise apps that need to stay extensible

## Getting started

1. **Prerequisites** — install the **.NET 10.0 SDK** or later.
2. **Install the template** from the repository root:

   ```bash
   dotnet new install .
   ```

3. **Create a new project:**

   ```bash
   dotnet new RA.Template -n YourProjectName
   ```

4. **Run the application:**

   ```bash
   cd YourProjectName
   dotnet restore
   dotnet run --project src/Presentation/YourProjectName.Api/YourProjectName.Api.csproj
   ```

5. **Access the API** — the default profile listens on `http://localhost:5039`. OpenAPI documentation is served at `/openapi-ui`, and health checks (when a database is configured) at `/health`.

## Next steps

- [Install the template](installation.md) and [create your first project](first-project.md)
- Read the [Architecture Overview](../architecture/overview.md) to understand the layer structure
- Read the [README](../../README.md) for the full user guide, template parameters, and customization examples
- Browse the source tree under `src/` to understand the layer structure
- Run the architecture tests in `tests/RaTemplate.ArchitectureTests/` to see the enforced conventions
