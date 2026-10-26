---
alwaysApply: true
description: >
  Enforces dependency direction, feature organization, and module boundary
  rules for .NET solution architecture.
color: purple
---

# RaTemplate — Architecture

Architecture, project structure, and module boundaries for the `RaTemplate` solution.

This document describes the code **as it exists today**, verified by reading every project file and
running `dotnet build`. Where `README.md` / `CLAUDE.md` disagree with the source, this
document follows the source and flags the drift in [Known gaps and boundary leaks](#5-known-gaps-and-boundary-leaks).

---

## 1. Architecture

### 1.1 Style and stack

Clean Architecture (layered, dependency-inverted), generated from `RaTemplate`.

| Concern | Choice |
|---|---|
| Runtime | .NET 10 (`net10.0`) |
| API style | ASP.NET Core Minimal APIs |
| Feature/CQRS | `RA.Utilities.Dispatcher` — `IRequest`/`IRequestHandler`/`IPipelineBehavior` + `AddMediator()` |
| Validation | FluentValidation, registered assembly-wide |
| Data access | EF Core 10, SQLite provider, per-provider `DbContext` abstractions |
| Contracts | `RaTemplate.Api.Contracts` (shared DTO project) |
| Docs UI | `Microsoft.AspNetCore.OpenApi` + Scalar at `/openapi-ui` |
| Logging | Serilog via `RA.Utilities.Logging.Core`, configured from `appsettings.json` |
| Health | `AspNetCore.HealthChecks` (`self`, SQLite) + UI client response writer |
| Tests | xUnit + NetArchTest.Rules (architecture enforcement only) |
| Analysis | SonarAnalyzer.CSharp, `AnalysisMode=All`, `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild` |
| Packages | Central Package Management (`Directory.Packages.props`) — `PackageReference` entries carry **no** `Version` |

### 1.2 Layer model

Dependencies point inward only. Nothing in Core knows anything in Infrastructure or Presentation exists.

```
Domain  ◄──  Application  ◄──  Persistence / Integration  ◄──  Infrastructure  ◄──  Api

                                       Api.Contracts  (standalone, referenced by Api only)
```

Reference graph, outermost project first:

```
RaTemplate.Api ──────────────────────── Presentation: composition root, middleware, endpoint mapping
│
├──►  RaTemplate.Api.Contracts ──────── Presentation: shared DTOs; references nothing
│
├──►  RaTemplate.Application ────────── Core: features, handlers, validators, data abstractions
│       │
│       └──►  RaTemplate.Domain ─────── Core: entities, domain constants; references nothing
│
└──►  RaTemplate.Infrastructure ─────── Infrastructure: thin aggregator, no logic of its own
        │
        ├──►  RaTemplate.Persistence ── Infrastructure: EF Core/SQLite, initializer, DB health checks
        │       └──►  RaTemplate.Application
        │
        └──►  RaTemplate.Integration ── Infrastructure: outbound HTTP clients, request/response logging
                └──►  RaTemplate.Application
```

`Persistence` and `Integration` are siblings — neither references the other, and both depend only on
`Application`. `Infrastructure` exists solely to give `Api` a single wiring entry point.

### 1.3 Actual project references

Read from the `.csproj` files — this is the ground truth the build enforces.

| Project | References |
|---|---|
| `RaTemplate.Domain` | *(none)* |
| `RaTemplate.Application` | `RaTemplate.Domain` |
| `RaTemplate.Persistence` | `RaTemplate.Application` |
| `RaTemplate.Integration` | `RaTemplate.Application` |
| `RaTemplate.Infrastructure` | `RaTemplate.Application`, `RaTemplate.Persistence`, `RaTemplate.Integration` |
| `RaTemplate.Api.Contracts` | *(none)* |
| `RaTemplate.Api` | `RaTemplate.Application`, `RaTemplate.Infrastructure`, `RaTemplate.Api.Contracts` |
| `RaTemplate.ArchitectureTests` | all seven `src` projects |

`RaTemplate.Api` reaches `RaTemplate.Persistence` only **transitively**, through `RaTemplate.Infrastructure`.
`Program.cs` nevertheless imports `RaTemplate.Persistence.Database` to call the dev-time initializer —
see [Known gaps](#5-known-gaps-and-boundary-leaks).

### 1.4 Composition root

`src/Presentation/RaTemplate.Api/Program.cs` is the only composition root. There is **no**
`StartupExtensions` class; service registration is split between inline chaining in `Program.cs` and
the extension classes under `RaTemplate.Api/ServiceConfiguration/`.

Registration order in `Program.cs`:

1. `builder.AddLoggingWithConfiguration()` — Serilog from the `Serilog` config section (`RA.Utilities.Logging.Core`)
2. `Configure<JsonOptions>` — adds `JsonStringEnumConverter`
3. `AddDefaultHealthChecks()` — `self` check tagged `live` (`HealthCheckExtensions`)
4. `AddOpenApi(configuration)` — binds `OpenApiInfoSettings`, adds default + value-object schema transformers (`OpenApiExtensions`)
5. `AddCorsPolicies(configuration, environment)` — single default policy (`CorsExtensions`)
6. `AddRaExceptionHandling()` + `AddProblemDetails()` — RFC 9457 error responses (`RA.Utilities.Api`)
7. `AddApplicationServices()` — validators from the Application assembly + `AddMediator()`
8. `AddInfrastructureServices(configuration)` — fans out to `AddPersistence(configuration)` and `AddIntegrationServices(configuration)`
9. `AddDefaultMiddlewares()` — default-headers + request/response logging middleware (`MiddlewareExtensions`)

Each layer exposes its own `DependencyInjection` static class with `IServiceCollection` extensions, so the
composition root never touches EF Core, `HttpClient`, or provider-specific types.

### 1.5 Request pipeline

After `builder.Build()`:

```
Development only      → RaTemplateDbInitializer.InitializeDatabaseAsync(app.Services)
Non-Production only   → app.UseOpenApi()   (MapOpenApi + Scalar at /openapi-ui)
always                → MapHealthCheckEndpoints()   (/health, /alive)
                        UseDefaultHeadersMiddleware()
                        UseLoggingMiddleware()
                        UseCors()
                        UseHttpsRedirection()
                        UseExceptionHandler()
                        app.MapEndpoints()          (feature endpoints from Application)
                        GET  /                      (sample: Pizza)
                        POST /                      (sample: Pizza echo)
                        POST /value-objects         (sample: ValueObjects echo)
```

The three sample endpoints at the bottom of `Program.cs` are template demos. They bypass the feature
pattern and are expected to be deleted once real features exist.

### 1.6 Data access

The Application layer owns the abstraction; Persistence owns the implementation. One `DbContext` per
database provider — currently SQLite only.

```
Application                          Persistence
IRaTemplateSqliteDbContext  ◄────────  RaTemplateSqliteDbContext (sealed, implements it)
  (Abstractions/Data)                  (Database/)
```

- `RaTemplateSqliteDbContext` applies configurations with `ApplyConfigurationsFromAssembly`, so any
  `IEntityTypeConfiguration<>` in the Persistence assembly is picked up automatically.
- `BaseEntitySaveChangesInterceptor` (from `RA.Utilities.Data.EntityFramework`) is registered scoped and
  attached in `ConfigureOptions<TContext>`; it stamps the audit fields on the `CoreEntity<TKey>` hierarchy.
- Development adds `LogTo(...)` on `DbLoggerCategory.Database.Command` plus `EnableSensitiveDataLogging()`.
- Connection string key: `ConnectionStrings:RaTemplateSqliteConnectionString` (`Data Source=RaTemplate.db`).
  Missing/blank throws via `ArgumentException.ThrowIfNullOrWhiteSpace`.
- `RaTemplateDbInitializer` runs **only** in Development and is destructive: `EnsureDeletedAsync()` then
  `EnsureCreatedAsync()`. There are no EF migrations in the solution.
- Health check: `AddSqlite(...)` on the same connection string.

### 1.7 Cross-cutting concerns

| Concern | Where it lives | Notes |
|---|---|---|
| Logging | `RA.Utilities.Logging.Core` + `Serilog` section of `appsettings.json` | Daily rolling files in `RaTemplate.Api/Logs/`, console sink, sensitive-data masking for `password` and `Authorization` |
| Request logging | `MiddlewareExtensions.AddDefaultMiddlewares()` | Body log cap 16 KB; ignores `/openapi-ui`, `/openapi`, `/health` |
| Exceptions | `AddRaExceptionHandling()` + `UseExceptionHandler()` + `AddProblemDetails()` | Problem-details responses |
| CORS | `CorsExtensions` | Development: any origin/header/method. Otherwise: only `Cors:AllowedOrigins`, fixed method list, **credentials never allowed** — use a bearer token. Empty list outside Development rejects every cross-origin call |
| Health | `HealthCheckExtensions` | `/health` (all checks, UI writer), `/alive` (tag `live`) |
| API docs | `OpenApiExtensions` | `/openapi` document, `/openapi-ui` Scalar UI, non-Production only |
| Validation | `AddValidatorsFromAssembly(Application)` | FluentValidation, runs through the feature pipeline |
| Auditing | `BaseEntitySaveChangesInterceptor` | `CreatedAt` / `LastModifiedAt` / `IsDeleted` / `CreatedBy` |
| Entity constraints | `RaTemplate.Domain/Constants` | `EntitiesConstraints` (currently empty) — max lengths and precision belong here, never as literals in EF configuration |
| Error codes | `RaTemplate.Domain/Constants/ErrorCodes.cs` | Extends `RA.Utilities.Core.Constants.BaseErrorCode` |

### 1.8 Shared kernel — `RA.Utilities` package map

The solution leans on the `RA.Utilities` library family for everything cross-cutting. Versions are pinned
centrally in `Directory.Packages.props`.

| Package | Used by | Provides |
|---|---|---|
| `RA.Utilities.Data.Entities` | Domain | `CoreEntity<TKey>`, `BaseEntity<TKey>`, `SoftDeleteEntity<TKey>`, `WriteEntity<TKey>`, `AuditableBaseEntity<TKey>` |
| `RA.Utilities.Core.ValueObjects` | Domain, Api.Contracts | `Email`, `SSN`, `Currency`, `Money` |
| `RA.Utilities.Feature` | Application | `IRequest`, `IRequestHandler`, `IPipelineBehavior`, `IEndpoint`, `AddMediator()` |
| `RA.Utilities.Data.Abstractions` | *(declared, not yet referenced)* | Data abstraction helpers |
| `RA.Utilities.Data.EntityFramework` | Persistence | `BaseEntitySaveChangesInterceptor`, value converters (`EmailConverter`, `SsnConverter`, `CurrencyConverter`), `HasMoney()` |
| `RA.Utilities.Integrations` | Integration | `RequestResponseLoggingHandler`, HTTP client registration extensions |
| `RA.Utilities.Api` | Api | `AddRaExceptionHandling`, default-headers and logging middleware, `MapEndpoints` |
| `RA.Utilities.Api.Response` | *(declared, not yet referenced)* | Typed response envelopes |
| `RA.Utilities.OpenApi` | Api | `OpenApiInfoSettings`, `AddDefaultsDocumentTransformer` |
| `RA.Utilities.Logging.Core` | Api | `AddLoggingWithConfiguration`, `UseLoggingMiddleware` |

---

## 2. Project structure

### 2.1 Repository layout

```
RaTemplate/
├── RaTemplate.sln                     # solution folders: Solution Items, src/{Core,Infrastructure,Presentation}, tests
├── global.json                       # SDK 10.0.400, rollForward latestFeature
├── Directory.Build.props             # net10.0, nullable, analysis, SonarAnalyzer for every project
├── Directory.Packages.props          # Central Package Management — all versions live here
├── .editorconfig                     # code style + analyzer rule tuning
├── AGENTS.md / CLAUDE.md             # agent routing and repo guidance
├── README.md                         # human-facing quickstart
├── .agents/skills/, .claude/         # agent skills and rules (e.g. new-entity)
├── src/
│   ├── Core/
│   │   ├── RaTemplate.Domain/
│   │   │   ├── AssemblyReference.cs
│   │   │   ├── Constants/
│   │   │   │   ├── EntitiesConstraints.cs
│   │   │   │   └── ErrorCodes.cs
│   │   │   └── Entities/
│   │   │       └── Event.cs
│   │   └── RaTemplate.Application/
│   │       ├── AssemblyReference.cs
│   │       ├── DependencyInjection.cs
│   │       └── Abstractions/Data/
│   │           └── IRaTemplateSqliteDbContext.cs
│   ├── Infrastructure/
│   │   ├── RaTemplate.Infrastructure/
│   │   │   ├── AssemblyReference.cs
│   │   │   └── DependencyInjection.cs
│   │   ├── RaTemplate.Persistence/
│   │   │   ├── AssemblyReference.cs
│   │   │   ├── DependencyInjection.cs
│   │   │   └── Database/
│   │   │       ├── RaTemplateSqliteDbContext.cs
│   │   │       └── RaTemplateDbInitializer.cs
│   │   └── RaTemplate.Integration/
│   │       ├── AssemblyReference.cs
│   │       └── DependencyInjection.cs
│   └── Presentation/
│       ├── RaTemplate.Api.Contracts/
│       │   └── AssemblyReference.cs
│       └── RaTemplate.Api/
│           ├── Program.cs            # composition root
│           ├── AssemblyReference.cs
│           ├── ServiceConfiguration/
│           │   ├── CorsExtensions.cs
│           │   ├── HealthCheckExtensions.cs
│           │   ├── MiddlewareExtensions.cs
│           │   └── OpenApiExtensions.cs
│           ├── Models/               # template demo models (Pizza, ValueObjects)
│           ├── Properties/launchSettings.json
│           ├── appsettings.json / appsettings.Development.json
│           ├── RaTemplate.Api.http
│           └── RaTemplate.db          # SQLite database (currently tracked in git)
└── tests/
    └── RaTemplate.ArchitectureTests/
        ├── Assemblies.cs, Namespaces.cs   # reflection roots for NetArchTest
        ├── DependencyTests.cs             # layer isolation
        ├── ApplicationTests.cs, HandlersTests.cs   # feature naming + sealing
        ├── EntitiesTests.cs               # entity conventions
        ├── PersistenceTests.cs            # EF configuration conventions
        └── ApiTests.cs                    # endpoint conventions
```

### 2.2 Project responsibilities

| Project | Layer | Responsibility | Must not contain |
|---|---|---|---|
| `RaTemplate.Domain` | Core | Entities, value-object usage, domain constants (constraints, error codes) | Any project reference, EF attributes, ASP.NET types, DTOs |
| `RaTemplate.Application` | Core | Features (`*Input` / `*Handler` / `*Output`), validators, pipeline decorators, endpoint definitions, `Abstractions/Data` interfaces | EF implementations, `HttpClient`, ASP.NET hosting types, Presentation types |
| `RaTemplate.Persistence` | Infrastructure | `DbContext` implementations, `IEntityTypeConfiguration<>` classes, save-changes interceptor wiring, DB initializer, DB health checks | Domain rules, Presentation types, Integration types |
| `RaTemplate.Integration` | Infrastructure | Outbound HTTP clients and delegating handlers | Persistence types, Presentation types |
| `RaTemplate.Infrastructure` | Infrastructure | Aggregator: calls `AddPersistence` + `AddIntegrationServices` so the API has one entry point | Business logic of its own — it is deliberately a thin wiring module |
| `RaTemplate.Api.Contracts` | Presentation | Shared request/response DTOs consumed by endpoints and clients | Any project reference; behaviour |
| `RaTemplate.Api` | Presentation | Host, `Program.cs` composition root, middleware, CORS/health/OpenAPI wiring, endpoint mapping | Domain rules, EF configuration, direct `DbContext` usage |
| `RaTemplate.ArchitectureTests` | Tests | Enforces every rule in this document | Production code |

Every `src` project contains an `AssemblyReference` marker type. It exists purely so the architecture
tests can resolve each assembly by reflection without hardcoding names.

---

## 3. Module boundaries

### 3.1 Forbidden dependency matrix

Enforced by `tests/RaTemplate.ArchitectureTests/DependencyTests.cs`. A cell marked ✗ means the row
assembly must have **no** dependency on the column assembly.

| From ↓ / To → | Domain | Application | Persistence | Integration | Infrastructure | Api | Api.Contracts |
|---|---|---|---|---|---|---|---|
| **Domain** | — | ✗ | ✗ | ✗ | ✗ | ✗ | ✗ |
| **Application** | allowed | — | ✗ | ✗ | ✗ | ✗ | ✗ |
| **Persistence** | allowed | allowed | — | ✗ | ✗ | ✗ | ✗ |
| **Integration** | allowed | allowed | ✗ | — | ✗ | ✗ | ✗ |
| **Infrastructure** | allowed | allowed | allowed | allowed | — | ✗ | ✗ |
| **Api** | allowed | allowed | *(transitive only)* | *(transitive only)* | allowed | — | allowed |
| **Api.Contracts** | ✗ | ✗ | ✗ | ✗ | ✗ | ✗ | — |

Notes:

- The `Api.Contracts` test deliberately omits `RaTemplate.Api` from its forbidden list, because NetArchTest
  matches by namespace **prefix** and `RaTemplate.Api` would also match `RaTemplate.Api.Contracts` itself.
  The project reference graph is what keeps it clean.
- `Persistence` and `Integration` are siblings: they must never reference each other. Anything both need
  belongs in `Application` (abstraction) or `Domain`.
- `Infrastructure` is the only module allowed to see both siblings; that is its whole purpose.

### 3.2 Boundary enforcement

`dotnet test` runs the following rules. Keep them green — they are the executable form of this document.

| Test file | Rule |
|---|---|
| `DependencyTests` | Layer isolation exactly as in §3.1 |
| `EntitiesTests` | Types in `RaTemplate.Domain.Entities` inherit `CoreEntity<>`; entities are `sealed`; nothing outside that namespace inherits `CoreEntity<>` |
| `ApplicationTests` / `HandlersTests` | `IRequest` implementations end with `Input` and are sealed; handler output types end with `Output`; handlers end with `Handler`; `IPipelineBehavior` implementations end with `Decorator`; `AbstractValidator<>` subclasses end with `Validator`; all of them sealed |
| `PersistenceTests` | `IEntityTypeConfiguration<>` implementations are non-public and sealed, live in `RaTemplate.Persistence.Configuration`, end with `Config`, and are named `<Entity>Config` |
| `ApiTests` | `IEndpoint` implementations depend on `RA.Utilities.Feature.Abstractions`; endpoints return a typed `IResult` containing an `RaTemplate.Api.Contracts` class (or `NoContent`/`Redirect`) |

`ApiTests` has a practical consequence: **endpoint responses must be declared in `RaTemplate.Api.Contracts`.**
Returning an anonymous object, a Domain entity, or an Api-local model from a feature endpoint fails the build
of the test suite.

### 3.3 Naming and placement conventions

| Artifact | Namespace / folder | Naming | Visibility |
|---|---|---|---|
| Entity | `RaTemplate.Domain.Entities` | singular PascalCase | `sealed class` |
| Entity constraints | `RaTemplate.Domain.Constants.EntitiesConstraints` | `<Entity>Constraints` | `static class` |
| Error codes | `RaTemplate.Domain.Constants` | — | extends `BaseErrorCode` |
| Feature request | `RaTemplate.Application.<Feature>` | `<Name>Input` | `sealed` |
| Feature response | `RaTemplate.Application.<Feature>` | `<Name>Output` | `sealed` |
| Feature handler | `RaTemplate.Application.<Feature>` | `<Name>Handler` | `sealed` |
| Pipeline decorator | `RaTemplate.Application.<Feature>` | `<Name>Decorator` | `sealed` |
| Validator | `RaTemplate.Application.<Feature>` | `<Name>Validator` | `sealed` |
| Data abstraction | `RaTemplate.Application.Abstractions.Data` | `IRaTemplate<Provider>DbContext` | `interface` |
| `DbContext` | `RaTemplate.Persistence.Database` | `RaTemplate<Provider>DbContext` | `sealed class` |
| EF configuration | `RaTemplate.Persistence.Configuration` | `<Entity>Config` | `internal sealed class` |
| API contract | `RaTemplate.Api.Contracts` | `<Name>Response` / `<Name>Request` | `class` |

Code style is enforced at build time by `.editorconfig` + `EnforceCodeStyleInBuild`: file-scoped namespaces,
explicit types (`var` only where the type is apparent), braces required, expression-bodied members for
operators/properties/accessors, XML docs on public members (`GenerateDocumentationFile=true`, and missing
docs become warnings-as-errors).

### 3.4 Where new code goes

**Adding a feature (endpoint + handler):**

1. `RaTemplate.Api.Contracts` — response/request DTOs
2. `RaTemplate.Application/<Feature>/` — `<Name>Input`, `<Name>Output`, `<Name>Handler`, `<Name>Validator`, endpoint class implementing `IEndpoint`
3. `RaTemplate.Application/Abstractions/Data` — extend the `DbContext` abstraction if new data is needed
4. Nothing in `Api` (features are discovered by `app.MapEndpoints()`), nothing in `Infrastructure`

**Adding an entity:**

1. `RaTemplate.Domain/Entities/<Entity>.cs` — `sealed`, correct base class from the ladder below
2. `RaTemplate.Domain/Constants/EntitiesConstraints/<Entity>Constraints.cs` — max lengths, precision
3. `RaTemplate.Application/Abstractions/Data/IRaTemplateSqliteDbContext.cs` — `DbSet<Entity> Entities { get; }` (plural)
4. `RaTemplate.Persistence/Database/RaTemplateSqliteDbContext.cs` — matching `DbSet`
5. `RaTemplate.Persistence/Configuration/<Entity>Config.cs` — `internal sealed`, Fluent API only, constants not literals

Entity base-class ladder (`RA.Utilities.Data.Entities`, root `CoreEntity<TKey>`):

| Properties needed | Base class |
|---|---|
| `Id` | `CoreEntity<TKey>` |
| `Id`, `CreatedAt` | `BaseEntity<TKey>` |
| `Id`, `CreatedAt`, `IsDeleted` | `SoftDeleteEntity<TKey>` |
| `Id`, `CreatedAt`, `LastModifiedAt` | `WriteEntity<TKey>` |
| `Id`, `CreatedAt`, `LastModifiedAt`, `CreatedBy`, `LastModifiedBy` | `AuditableBaseEntity<TKey>` |

**Adding an external HTTP dependency:** everything goes in `RaTemplate.Integration` behind an interface
declared in `RaTemplate.Application`. Never register an `HttpClient` in `Api` or `Application`.

---

## 4. Build and run

```bash
dotnet restore
dotnet build
dotnet test                                                        # architecture rules
dotnet run --project src/Presentation/RaTemplate.Api/RaTemplate.Api.csproj              # http://localhost:5039
dotnet run --project src/Presentation/RaTemplate.Api/RaTemplate.Api.csproj --launch-profile https
```

Endpoints: `/openapi-ui` (Scalar, non-Production), `/health`, `/alive`.

Because `Directory.Build.props` sets `TreatWarningsAsErrors` and `CodeAnalysisTreatWarningsAsErrors`,
any analyzer warning fails the build. Suppress deliberately with `#pragma warning disable <id>` around the
narrowest possible scope, as `MiddlewareExtensions` and `RaTemplateDbInitializer` already do.

---

## 5. Known gaps and boundary leaks

Verified against the working tree on 2026-10-22.

**The solution does not currently build.**

```
src/Presentation/RaTemplate.Api/ServiceConfiguration/OpenApiExtensions.cs(2,21):
error CS0234: The type or namespace name 'OpenApi' does not exist in the namespace 'RaTemplate.Api'
```

`OpenApiExtensions.cs` imports `RaTemplate.Api.OpenApi` and calls
`options.AddSchemaTransformer<ValueObjectSchemaTransformer>()`, but no `OpenApi/` folder or
`ValueObjectSchemaTransformer` type exists in the project. Either add the transformer under
`RaTemplate.Api/OpenApi/` or drop both the `using` and the call. Until this is fixed, `dotnet test`
cannot run, so **none of the architecture rules in §3.2 are actually being verified**.

Other gaps, in rough priority order:

1. **`Event` is not mapped.** `DbSet<Event>` is commented out in both `IRaTemplateSqliteDbContext` and
   `RaTemplateSqliteDbContext`, and `RaTemplate.Persistence/Configuration/` does not exist. The latest
   commit ("Implement Event entity") added the entity but not its persistence wiring, so no table is
   created for it.
2. **Application layer is empty.** No features, handlers, validators, or decorators exist yet — only
   `DependencyInjection` and the data abstraction.
3. **`Api.Contracts` is empty.** Only `AssemblyReference.cs`. `ApiTests` requires endpoint results to
   carry contract types, so the first real feature must add its DTOs here.
4. **Skill/test contradiction.** `.agents/skills/new-entity.md` instructs
   `src/Core/RaTemplate.Persistence/Configurations/<Entity>Configuration.cs`, but `PersistenceTests`
   requires namespace `RaTemplate.Persistence.Configuration` and the suffix `Config`
   (`<Entity>Config.cs`). Following the skill as written fails three tests. The skill also places
   Persistence under `src/Core/` — it lives under `src/Infrastructure/`. **Follow §3.3, not the skill.**
5. **Documentation drift.** `README.md` and `CLAUDE.md` describe a `StartupExtensions` class with
   `AddServices`/`UsePipelines`, multi-provider contexts (SqlServer/Oracle/Postgres), and JWT
   authorization wiring. None of that exists: registration is inline in `Program.cs`, SQLite is the only
   provider, and there is no authorization code.
6. **Composition root crosses a layer.** `Program.cs` imports `RaTemplate.Persistence.Database` to call
   `RaTemplateDbInitializer.InitializeDatabaseAsync`, resolved through the transitive
   `Api → Infrastructure → Persistence` reference. `DependencyTests` does not catch this (it only inspects
   the Infrastructure/Persistence assemblies), but it is a real inward-boundary leak. Cleaner options:
   expose the initializer through an `RaTemplate.Infrastructure` extension, or add an explicit
   `Api → Persistence` reference and accept it as composition-root privilege.
7. **No migrations.** Development startup does `EnsureDeleted` + `EnsureCreated`, wiping data on every
   run. Fine for a scratch project, unsafe the moment real data or a non-Development environment appears.
8. **Database file is committed.** `src/Presentation/RaTemplate.Api/RaTemplate.db` is tracked in git —
   `.gitignore` has no `*.db` rule. Add one and untrack the file.
9. **Demo code in the host.** `Models/Pizza.cs`, `Models/ValueObjects.cs`, and the three sample endpoints
   in `Program.cs` are template leftovers that bypass the feature pattern.
10. **Unused surface.** `AddIntegrationServices(configuration)` ignores its `configuration` parameter and
    registers no named clients. `RA.Utilities.Data.Abstractions` and `RA.Utilities.Api.Response` are
    version-declared in `Directory.Packages.props` but referenced by no project. `ErrorCodes` is a plain
    non-sealed `public class`; it is the only extensible public type in the solution and reads as an
    oversight rather than a design choice (making it `sealed` or `static` would not change behaviour).
