# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

This is a .NET solution template (`RA.CleanArchitecture.Template`) that scaffolds Web API projects following Clean Architecture principles. It is itself a real, buildable .NET solution — the source code is the template content. Projects are named `RaTemplate.*` in source; the `sourceName` in `template.json` (`VsClArchTemplate`) is replaced when a user scaffolds a new project.

Target framework: **.NET 10.0** (`net10.0`).

## Build and test commands

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
dotnet run --project src/Web/RaTemplate.Api/RaTemplate.Api.csproj

# Run all tests
dotnet test

# Run a single test
dotnet test --filter "FullyQualifiedName=Namespace.ClassName.TestMethodName"

# Install the template locally (from repo root)
dotnet new install .

# Uninstall the template
dotnet new uninstall RA.CleanArchitecture.Template
```

The API starts on `https://localhost:7001` by default. OpenAPI docs are at `/openapi-ui`.

## Solution structure

```
src/
  Core/
    RaTemplate.Domain/          # Entities, value objects — no project dependencies
    RaTemplate.Application/     # CQRS handlers, DTOs, application services — depends on Domain
  Infrastructure/
    RaTemplate.Infrastructure/  # Wires up other infra projects — depends on Application, Persistence, Integration
    RaTemplate.Persistence/     # EF Core DbContext, repositories — depends on Application
    RaTemplate.Integration/     # HTTP client integrations — depends on Application
  Presentation/
    RaTemplate.Api/             # ASP.NET Core host — depends on Application and Infrastructure
    RaTemplate.Api.Contracts/   # Shared API contracts — no project dependencies
tests/
  RaTemplate.ArchitectureTests/ # NetArchTest-based architecture enforcement — depends on all src projects
```

## Architecture patterns

**Clean Architecture layers (inner to outer):** Domain → Application → Infrastructure → Web. Dependencies only point inward (e.g., Infrastructure depends on Application, not vice versa).

**Dependency injection wiring pattern:** Each layer exposes a public `*ServiceRegistration` or `*DependencyInjection` class with extension methods on `IServiceCollection`. The API's `StartupExtensions.AddServices` orchestrates registration by calling each layer in order:

```
AddApplicationServices() → AddInfrastructureServices(configuration)
```

Within `AddInfrastructureServices`, conditional compilation symbols (`#if UseIntegrations`, `#if UseAnyDatabase`) gate optional infrastructure projects.

**Service registration entry points:**
- `RaTemplate.Application.ApplicationServiceRegistration.AddApplicationServices()` — registers Mediator
- `RaTemplate.Infrastructure.InfrastructureServiceRegistration.AddInfrastructureServices()` — gates and calls Integration + Persistence
- `RaTemplate.Persistence.PersistenceDependencyInjection.AddPersistence()` — DbContext, health checks, repositories
- `RaTemplate.Integration.IntegrationServiceRegistration.AddIntegrationServices()` — HTTP client with logging handler

**Feature/Mediator pattern:** The application layer uses `RA.Utilities.Feature` for CQRS via `services.AddMediator()`. Features are discovered by calling `services.AddEndpoints(Assembly.GetExecutingAssembly())` which maps feature handlers to minimal API endpoints.

**Conditional compilation symbols** (set by `template.json` when scaffolding, or defined manually when building the template source):
- `UseAuthorization` — gates JWT auth services
- `UseIntegrations` — gates `RaTemplate.Integration` project
- `UseAnyDatabase` — gates `RaTemplate.Persistence` project
- `UseScalarUI` — gates Scalar API docs (vs Swagger)

These appear as `#if UseAuthorization` preprocessor directives in source. When building the template source directly (not via `dotnet new`), these symbols are **not defined** by default — conditional code is excluded unless you define the symbols.

## Central package management

The repo uses `ManagePackageVersionsCentrally` in `Directory.Packages.props`. All `PackageReference` elements in `*.csproj` files omit the `Version` attribute — versions are defined centrally in `Directory.Packages.props`. Add new package versions there, not in individual project files.

Key RA.Utilities packages (all `10.0.0-rc.2`):
- `RA.Utilities.Feature` — Mediator/CQRS
- `RA.Utilities.Api` / `RA.Utilities.Api.Middlewares` — exception handling, HTTP logging, default headers
- `RA.Utilities.OpenApi` — OpenAPI document transformers
- `RA.Utilities.Authentication.JwtBearer` / `RA.Utilities.Authorization` — JWT auth
- `RA.Utilities.Data.EntityFramework` / `RA.Utilities.Data.Entities` / `RA.Utilities.Data.Abstractions` — persistence base classes
- `RA.Utilities.Logging.Core` — structured logging
- `RA.Utilities.Integrations` — HTTP client integration helpers

## Code quality defaults

- `TreatWarningsAsErrors` and `CodeAnalysisTreatWarningsAsErrors` are enabled
- Static analysis: `SonarAnalyzer.CSharp` (with many rules relaxed in `.editorconfig`)
- Code style: file-scoped namespaces, explicit types (no `var` except when type is apparent), expression-bodied members for operators/properties/accessors
