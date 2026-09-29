# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

A .NET 10 (`net10.0`) Web API solution following Clean Architecture principles. Dependencies only point inward: **Domain → Application → Infrastructure → Presentation**.

This solution was generated from the `RA.CleanArchitecture.Template` template. Depending on the options chosen at generation time, some projects may be absent:

- `RaTemplate.Persistence` (and `RaTemplate.Application/Abstractions/Data`) when no database provider was selected
- `RaTemplate.Integration` when HTTP integrations were disabled
- JWT authorization wiring (and `AuthorizationExtensions.cs`) when authorization was disabled

## Build and test commands

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
dotnet run --project src/Presentation/RaTemplate.Api/RaTemplate.Api.csproj

# Run all tests
dotnet test

# Run a single test
dotnet test --filter "FullyQualifiedName=Namespace.ClassName.TestMethodName"
```

The API listens on `http://localhost:5039` with the default profile (`https://localhost:7202` with `--launch-profile https`). Scalar API documentation is at `/openapi-ui` (mapped outside the Production environment); health checks are at `/health` when a database provider is configured.

## Solution structure

```
src/
  Core/
    RaTemplate.Domain/          # Entities, domain constants — no project dependencies
    RaTemplate.Application/     # Feature requests, handlers, validators, DTOs — depends on Domain
  Infrastructure/
    RaTemplate.Infrastructure/  # Wires up other infra projects — depends on Application (+ Persistence, Integration)
    RaTemplate.Persistence/     # EF Core DbContext, entity configurations, initializer — depends on Application
    RaTemplate.Integration/     # HTTP client integrations — depends on Application
  Presentation/
    RaTemplate.Api/             # ASP.NET Core host — depends on Application, Infrastructure, Api.Contracts
    RaTemplate.Api.Contracts/   # Shared API contracts — no project dependencies
tests/
  RaTemplate.ArchitectureTests/ # NetArchTest-based architecture enforcement — depends on all src projects
```

## Architecture patterns

**Composition root:** `RaTemplate.Api.StartupExtensions` registers everything (`AddServices`) and configures the pipeline (`UsePipelines`). `Program.cs` runs database initialization on startup in the Development environment.

**Service registration entry points:** Each layer exposes extension methods on `IServiceCollection`:

- `RaTemplate.Application.DependencyInjection.AddApplicationServices()` — FluentValidation validators + Mediator
- `RaTemplate.Infrastructure.DependencyInjection.AddInfrastructureServices(configuration)` — calls the Persistence and Integration registrations
- `RaTemplate.Persistence.DependencyInjection.AddPersistence(configuration)` — per-provider DbContexts, save-changes interceptor, health checks; reads the connection strings `ConnectionStrings:RaTemplate{SqlServer|Oracle|Postgres|Sqlite}ConnectionString`
- `RaTemplate.Integration.DependencyInjection.AddIntegrationServices(configuration)` — HTTP client with a request/response logging handler

**Feature/Mediator pattern:** The application layer uses `RA.Utilities.Feature` for CQRS via `services.AddMediator()`. Feature endpoints are discovered and mapped with `services.AddEndpoints(Assembly.GetExecutingAssembly())` in the API's `StartupExtensions`. Validators are FluentValidation `AbstractValidator<T>` implementations and are registered from the Application assembly.

**Data access:** The Application layer depends on per-provider abstractions such as `IRaTemplateSqlServerDbContext` (`RaTemplate.Application.Abstractions.Data`, one interface per selected database provider); the EF Core implementations live in `RaTemplate.Persistence`. Entities derive from `CoreEntity<TKey>` and saves are audited through `BaseEntitySaveChangesInterceptor`.

**Test-enforced conventions** (in `RaTemplate.ArchitectureTests` — keep these green when adding code):

- Feature requests end with `Input` and are sealed; their output types end with `Output`
- Handlers end with `Handler`, pipeline decorators with `Decorator`, validators with `Validator` — and all are sealed
- Domain entities live in `RaTemplate.Domain.Entities`, inherit `CoreEntity<>`, and are sealed
- EF entity configurations live in `RaTemplate.Persistence.Configuration`, are internal and sealed, and follow the `<Entity>Config` naming convention
- Layer boundaries are enforced with NetArchTest dependency rules

## Central package management

The solution uses `ManagePackageVersionsCentrally` in `Directory.Packages.props`. All `PackageReference` elements in `*.csproj` files omit the `Version` attribute — versions are defined centrally. Add new package versions there, not in individual project files.

## Code quality defaults

- `TreatWarningsAsErrors`, `CodeAnalysisTreatWarningsAsErrors`, and `EnforceCodeStyleInBuild` are enabled
- Static analysis: `SonarAnalyzer.CSharp` with many rules relaxed in `.editorconfig`
- Code style: file-scoped namespaces, explicit types (no `var` except when the type is apparent), braces required, expression-bodied members for operators/properties/accessors
