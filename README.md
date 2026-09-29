# RA.CleanArchitecture.Template

**RA.Clean.Architecture.Template** is a robust .NET solution template designed to streamline development using Clean Architecture principles.
Provides a well-structured starting point for building scalable and maintainable .NET applications.

This approach emphasizes a separation of concerns, placing business logic at the center of the application and making it independent of infrastructure details like databases or external services.

The template is highly configurable, allowing developers to tailor the generated project to their specific needs.

## 🔑 Key Features
  * **.NET 10**: It is set up to use `.NET 10`.
  * **Authorization**: You can include or exclude JWT (JSON Web Token) based authorization.
  * **HTTP Client Integrations**: There is an option to add a pre-configured infrastructure for making HTTP calls to other services. The `DependencyInjection.cs` file shows that this includes logging for requests and responses.
  * **⁠Built-in patterns**: Implements common design patterns like CQRS, Dependency Injection, and Mediator.
  * **OpenAPI Documentation**: Scalar, a modern and interactive API documentation UI, is preconfigured and served at `/openapi-ui`.
  * **Persistence Layer**: The template offers a flexible data access layer built on Entity Framework Core, with a choice of **SQL Server**, **Oracle**, **PostgreSQL**, or **SQLite** — multiple providers can be selected at once (one `DbContext` per provider) — or no database at all.
  * **Modular structure**: Promotes modular development with clear boundaries between core logic and external dependencies.
  * **⁠Ready-to-use setup**: Includes preconfigured logging, validation, exception handling, and API documentation (Scalar).
  * **Extensibility**: Designed to be easily extended for real-world projects, whether you're building REST APIs, microservices, or enterprise apps.
  * **Architecture tests**: A NetArchTest-based test project enforces layer boundaries, entity conventions, and feature naming rules from day one.

## 🚀 Ideal For
  * ⁠Developers looking for a clean, opinionated.NET template
  * ⁠Teams adopting Clean Architecture for long-term scalability
  * ⁠Projects that require separation of concerns and testability from day one

The project structure is modular: if you disable `UseIntegrations`, the entire `RaTemplate.Integration` project is excluded; if you choose `None` as the database, the `RaTemplate.Persistence` project is excluded; and if you disable `UseAuthorization`, the JWT wiring is left out. The generated solution stays clean and free of unused code.

## 🚀 Getting Started: A User Guide

This guide will walk you through installing and using the template to create a new Web API project.

### 1. Prerequisites

Make sure you have the **.NET 10.0 SDK** or a later version installed on your machine.

### 2. Installation

You can install the template directly from the source code repository or from NuGet once it's published.

*   **Local Installation (from source):**
    Clone the repository and run the following command from the root directory of the template project:
    ```bash
    dotnet new install .
    ```

*   **NuGet Installation:**
    Once published, you can install it using this command:
    ```bash
    dotnet new install RA.CleanArchitecture.Template
    ```

### 3. Creating a New Project

After installation, you can create a new project using the `dotnet new` command.

The basic command is:
```bash
dotnet new RA.Template -n YourProjectName
```
This will create a new solution in a folder named `YourProjectName` with the default settings (JWT Authorization, EF Core with SQL Server, Scalar UI, and HTTP Integrations).

### 4. Customizing Your Project with Parameters

You can customize the generated project by passing parameters to the `dotnet new` command.

**Example 1: Project with EF Core on Oracle and no Authorization**

This command scaffolds a project that uses Entity Framework Core with an Oracle database and disables JWT authorization.

```bash
dotnet new RA.Template -n MyOracleApi --database EfOracle --UseAuthorization false
```

**Example 2: A minimal API without Persistence or Integrations**

```bash
dotnet new RA.Template -n MyMinimalApi --database None --UseIntegrations false
```

**Example 3: Project with multiple database providers**

The `Database` parameter is multi-select: repeat the `--database` flag to get one `DbContext`, connection string, health check, and DI registration per provider. Select `None` alone when you do not want persistence.

```bash
dotnet new RA.Template -n MyMultiDbApi --database EfOracle --database EfPostgres
```

### 5. Running Your New Application

1.  **Navigate to the project directory**:
    `cd YourProjectName`
2.  **Restore Dependencies**:
    `dotnet restore`
3.  **Configure Settings**: If your project uses a persistence layer, open `src/Presentation/YourProjectName.Api/appsettings.json` and set the connection string for each selected provider, e.g. `ConnectionStrings:YourProjectNameSqlServerConnectionString` (the keys are renamed along with the project).
4.  **Run the application**:
    `dotnet run --project src/Presentation/YourProjectName.Api/YourProjectName.Api.csproj`
5.  **Access the API**: The default profile listens on `http://localhost:5039` (use `--launch-profile https` for `https://localhost:7202`). OpenAPI documentation is served at `http://localhost:5039/openapi-ui`, and health checks (when a database is configured) at `http://localhost:5039/health`.

### 6. Uninstalling the Template

To remove the template from your machine, run the following command:
```bash
dotnet new uninstall RA.CleanArchitecture.Template
```

## 🌳 RA.CleanArchitecture.Template Source Tree

The repository is both the template source and a buildable solution. A generated project has the same structure, with `RaTemplate` replaced by your project name and the optional projects omitted according to the selected parameters.

```
RA.CleanArchitecture.Template/
├── .template.config/
│   ├── template.json
│   ├── dotnetcli.host.json
│   └── ide.host.json
├── src/
│   ├── Core/
│   │   ├── RaTemplate.Domain/
│   │   │   ├── Constants/
│   │   │   └── AssemblyReference.cs
│   │   └── RaTemplate.Application/
│   │       ├── Abstractions/Data/
│   │       ├── AssemblyReference.cs
│   │       └── DependencyInjection.cs
│   ├── Infrastructure/
│   │   ├── RaTemplate.Infrastructure/
│   │   │   ├── AssemblyReference.cs
│   │   │   └── DependencyInjection.cs
│   │   ├── RaTemplate.Integration/
│   │   │   ├── AssemblyReference.cs
│   │   │   └── DependencyInjection.cs
│   │   └── RaTemplate.Persistence/
│   │       ├── Database/
│   │       ├── AssemblyReference.cs
│   │       └── DependencyInjection.cs
│   └── Presentation/
│       ├── RaTemplate.Api/
│       │   ├── Extensions/
│       │   ├── Properties/launchSettings.json
│       │   ├── Program.cs
│       │   ├── StartupExtensions.cs
│       │   └── appsettings.json
│       └── RaTemplate.Api.Contracts/
│           └── AssemblyReference.cs
├── tests/
│   └── RaTemplate.ArchitectureTests/
├── Directory.Build.props
├── Directory.Packages.props
├── RaTemplate.sln
├── README-template.md
└── CLAUDE-template.md
```
### Explanation of the Structure:
  * **`.template.config/`**: Holds the template metadata (`template.json`, `dotnetcli.host.json`, and `ide.host.json`), which define the parameters, conditional inclusions, and renames used by `dotnet new`.
  * **`src/`**: This is the main source code directory.
  * **`Core/RaTemplate.Domain/`**: Contains business entities, constants, and domain logic, with no dependencies on other layers.
  * **`Application/RaTemplate.Application/`**: This layer orchestrates the domain logic. It contains feature handlers, validators, DTOs, and abstractions for infrastructure concerns (like the per-provider `IRaTemplateSqlServerDbContext`). It depends on the Domain layer.
  * **`Infrastructure/`**: This layer contains implementations for external concerns.
    * **`RaTemplate.Infrastructure/`**: A central project for wiring up the other infrastructure components.
    * **`RaTemplate.Integration/`**: Contains services for communicating with external APIs, including a request/response logging HTTP handler. Excluded when `UseIntegrations` is disabled.
    * **`RaTemplate.Persistence/`**: Implements the data access logic using Entity Framework Core (SQL Server, Oracle, PostgreSQL, or SQLite — one `DbContext` per selected provider). Excluded when the database is set to `None`.
  * **`Presentation/RaTemplate.Api/`**: This is the entry point of your application—the API project. It handles HTTP requests, routing, and calls into the Application layer. It depends on the Application and Infrastructure layers for dependency injection setup.
  * **`Presentation/RaTemplate.Api.Contracts/`**: Shared request/response contracts with no project dependencies.
  * **`tests/RaTemplate.ArchitectureTests/`**: NetArchTest-based tests that enforce layer dependencies and naming conventions.
  * **`README-template.md` / `CLAUDE-template.md`**: Renamed to `README.md` / `CLAUDE.md` in generated projects; the repository's own `README.md` and `CLAUDE.md` are excluded from scaffolding.

## 🧠 Summary
In summary, **RA.CleanArchitecture.Template** template provides a robust and customizable foundation for developing modern, maintainable, and scalable .NET Web APIs.

## ⚙️ Template Parameters

| Parameter | Display Name | Description | Type | Default Value | Available Choices |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `Framework` | .NET Target Framework | Select the target framework for the project. | Choice | `net10.0` | `.NET 10` |
| `UseAuthorization` | JWT Authorization | Includes JWT-based authorization services and middleware. | Boolean | `true` | `true`, `false` |
| `UseIntegrations` | Use HTTP Client Integration? | Adds infrastructure for building and consuming external HTTP services. | Boolean | `true` | `true`, `false` |
| `OpenApiUI` | OpenApi documentation UI. | Selects the UI for API documentation. Currently only Scalar is available. | Choice | `scalar` | `scalar` |
| `Database` | Database provider | Selects the data access technology. Multi-select: repeat the flag to select several providers (one `DbContext` per provider). CLI aliases: `--database`, `-db`. | Choice (multi-select) | `EfSqlServer` | `EfSqlServer`, `EfOracle`, `EfPostgres`, `EfSqlite`, `None` |

> **Note:** Multi-select choices are passed by repeating the option (`--database EfOracle --database EfPostgres`); a single comma-separated token is rejected. `None` must be selected on its own and excludes the Persistence project.
