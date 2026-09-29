# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

This repository is a .NET solution template (`identity`: `RA.CleanArchitecture.Template`, `shortName`: `RA.Template`) that scaffolds Clean Architecture Web API solutions. The repository is itself a real, buildable .NET 10 (`net10.0`) solution — the source code doubles as the template content. Projects are named `RaTemplate.*` in the source; `sourceName` (`RaTemplate`) is replaced in all file paths and file contents when a user scaffolds a project.

Key `.template.config/template.json` facts:

- `sourceName` / `defaultName`: `RaTemplate` (replaced everywhere on scaffold)
- `sources.exclude`: repo docs (`README.md`, `CLAUDE.md`), `.template.config/**`, `bin`/`obj`, `.claude/**`, `.vscode/**`
- `sources.rename`: `README-template.md` → `README.md`, `CLAUDE-template.md` → `CLAUDE.md`
- `modifiers` exclude optional content per symbol: Persistence + `Application/Abstractions/Data` + `PersistenceTests.cs` when `!UseAnyDatabase`; per-provider `DbContext`/interface files (+ `Schemas.cs` for SQL Server) when the provider is not selected; Integration when `!UseIntegrations`; `AuthorizationExtensions.cs` when `!UseAuthorization`
- `primaryOutputs`: the API csproj and `README-template.md`; post-actions restore packages and open the generated README
- `dotnetcli.host.json` exposes `--database` / `-db` aliases; `ide.host.json` exposes the boolean parameters in Visual Studio

## Symbols

Parameter symbols: `Framework` (`net10.0`), `UseAuthorization` (bool), `UseIntegrations` (bool), `OpenApiUI` (choice, currently `scalar` only), `Database` (multi-select choice via `allowMultipleValues`: `EfSqlServer` | `EfOracle` | `EfPostgres` | `EfSqlite` | `None`; CLI syntax is repeating the flag, e.g. `--database EfOracle --database EfPostgres`).

Computed symbols (from those parameters): `UseScalarUI`, `UseEfSqlServer`, `UseEfOracle`, `UseEfPostgres`, `UseEfSqlite`, `UseAnyDatabase`.

## Conditional compilation model (IMPORTANT)

Conditional markers are stripped or kept by the template engine at scaffold time. Write them so that **the repository itself still builds**:

- **C# files**: use comment markers `//#if (SYMBOL)` / `//#endif` at any indentation. The engine removes the gated regions when the symbol is false (including the marker lines), and the repository build compiles the markers away as ordinary comments — so **all branches must always compile in the repo** (the repo build is a union build of every branch). Never use plain `#if` in C# files: the repo would compile only one branch and the other would silently rot.
- **XML (csproj, appsettings)**: use `<!--#if (SYMBOL) -->` / `<!--#endif -->`. These are inert comments for MSBuild but are processed by the engine.
- **Solution file (`RaTemplate.sln`)**: plain `#if (SYMBOL)` at column 0 is supported by the engine and is inert to tooling; this is the only place plain `#if` is used.
- **Markdown**: do not use conditional markers; describe optional parts in prose instead.

Conditional regions nest (e.g. `UseIntegrations` inside `UseAnyDatabase` in the architecture tests), so verification must cover the combinations that exercise the nesting.

**Blank-line hazard:** when the engine strips a gated region, surviving blank lines must not collapse into a double blank line — the repo's `.editorconfig` treats IDE2000 (multiple blank lines) as an error. Conventions used in this repo:

- exactly one blank line **before** a `//#if` marker, never a blank line immediately after;
- when two gated regions are adjacent, put the separating blank line **inside** the first region (before its `//#endif`) and place `//#endif` immediately followed by the next `//#if`.

**Multi-select choice semantics (empirically verified with a probe template):** in computed/condition expressions on an `allowMultipleValues` choice, `==` is a *membership test* (`(Database == "EfSqlServer")` is true iff `EfSqlServer` is among the selected values) and `!=` is non-membership. `Contains` does **not** work — `Db.Contains("X")` evaluated false for every selection. Boolean combinations (`||`, `&&`) of `==`-based tests work, so `UseAnyDatabase` is a 4-way OR of the per-provider computed symbols.

## Build and test commands

```bash
# Restore dependencies
dotnet restore

# Build the solution (union build — every conditional branch must compile)
dotnet build

# Run the API
dotnet run --project src/Presentation/RaTemplate.Api/RaTemplate.Api.csproj

# Run the architecture tests
dotnet test

# Install / refresh the template locally (from repo root)
dotnet new install . --force

# Uninstall the template
dotnet new uninstall RA.CleanArchitecture.Template
```

## Verifying template changes

After changing template content, verify by scaffolding real projects and building them:

```bash
dotnet new install . --force
dotnet new RA.Template -n Sample -o /private/tmp/sample --database EfOracle --UseAuthorization false
dotnet build /private/tmp/sample/Sample.sln
dotnet test /private/tmp/sample/Sample.sln
```

Notes:

- On macOS `/tmp` is a symlink to `/private/tmp`. Build generated projects via the canonical `/private/tmp/...` path (or from their own directory with a relative solution name); otherwise `.editorconfig` analyzer settings fail to match and produce false warnings/errors.
- Test at least one project per option: each `Database` provider, a multi-provider combo (e.g. `--database EfOracle --database EfPostgres`), `--database None --UseAuthorization false --UseIntegrations false`, and the combinations that exercise nested conditional regions.
- Keep the installed template fresh with `dotnet new install . --force` so local scaffolds reflect the working tree.
- CLI option names are case-sensitive: the `Database` symbol is exposed as `--database` / `-db` (from `dotnetcli.host.json`); other symbols keep their PascalCase names (`--Framework`, `--UseAuthorization`, `--UseIntegrations`, `--OpenApiUI`).

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

**Feature/Mediator pattern:** The application layer uses `RA.Utilities.Feature` for CQRS via `services.AddMediator()`. Feature endpoints are discovered and mapped with `services.AddEndpoints(Assembly.GetExecutingAssembly())` in the API's `StartupExtensions`.

The architecture tests enforce these conventions (keep them green in the repo's union build as well as in generated projects):

- Feature requests end with `Input` and are sealed; their output types end with `Output`
- Handlers end with `Handler`, pipeline decorators with `Decorator`, validators with `Validator` — and all are sealed
- Domain entities live in `RaTemplate.Domain.Entities`, inherit `CoreEntity<>`, and are sealed
- EF entity configurations live in `RaTemplate.Persistence.Configuration`, are internal and sealed, and follow the `<Entity>Config` naming convention
- Layer boundaries are enforced with NetArchTest dependency rules; note `HaveDependencyOnAny` matches dependency namespaces by **prefix** and counts same-assembly references, so forbidding `RaTemplate.Api` inside the tests would also match the tests' own `RaTemplate.Api.Contracts` types

## Central package management

The repo uses `ManagePackageVersionsCentrally` in `Directory.Packages.props`. All `PackageReference` elements in `*.csproj` files omit the `Version` attribute — versions are defined centrally. Add new package versions there, not in individual project files. Most `RA.Utilities` packages are `10.0.0-rc.2`; `RA.Utilities.Data.EntityFramework`, `RA.Utilities.Data.Entities`, and `RA.Utilities.Core.ValueObjects` use their own versions — check `Directory.Packages.props` for the current values.

## Code quality defaults

- `TreatWarningsAsErrors`, `CodeAnalysisTreatWarningsAsErrors`, `EnforceCodeStyleInBuild`, and `AnalysisMode=All` are enabled via `Directory.Build.props`; a build must stay at zero warnings
- Static analysis: `SonarAnalyzer.CSharp` with many rules relaxed in `.editorconfig`
- Code style: file-scoped namespaces, explicit types (no `var` except when the type is apparent), braces required, expression-bodied members for operators/properties/accessors
