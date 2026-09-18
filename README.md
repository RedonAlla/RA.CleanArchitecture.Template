# RA.CleanArchitecture.Template

**RA.Clean.Architecture.Template** is a robust .NET solution template designed to streamline development using Clean Architecture principles.
Provides a well-structured starting point for building scalable and maintainable .NET applications.

This approach emphasizes a separation of concerns, placing business logic at the center of the application and making it independent of infrastructure details like databases or external services.

The template is highly configurable, allowing developers to tailor the generated project to their specific needs.

## 🔑 Key Features
  * **.NET Framework**: It is set up to use `.NET 10`.
  * **Authorization**: You can include or exclude JWT (JSON Web Token) based authorization.
  * **HTTP Client Integrations**: There is an option to add a pre-configured infrastructure for making HTTP calls to other services. The `IntegrationServiceRegistration.cs` file shows that this includes logging for requests and responses.
  * **⁠Built-in patterns**: Implements common design patterns like CQRS, Dependency Injection, and Mediator.
  * **OpenAPI Documentation**: You can choose the UI for your API documentation:
    * **Scalar**: A modern, interactive API documentation tool.
    * **Swagger**: A more traditional and widely-used option.
  * **Persistence Layer**: The template offers a flexible data access layer with multiple choices that can be combined:
    * **ORM**: Entity Framework Core or Dapper.
    * **Database**: SQL Server or Oracle.
  * **Modular structure**: Promotes modular development with clear boundaries between core logic and external dependencies.
  * **⁠Ready-to-use setup**: Includes preconfigured logging, validation, exception handling, and API documentation (Swagger).
  * **Extensibility**: Designed to be easily extended for real-world projects, whether you're building REST APIs, microservices, or enterprise apps.

## 🚀 Ideal For
  * ⁠Developers looking for a clean, opinionated.NET template
  * ⁠Teams adopting Clean Architecture for long-term scalability
  * ⁠Projects that require separation of concerns and testability from day one

The project structure is modular. For instance, if you disable `UseIntegrations`, the entire `RaTemplate.Integration` project is excluded. Similarly, the persistence and authorization components are only included if you select them, keeping the final solution clean and free of unused code.

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

**Example 1: Project with Dapper, Oracle, and no Authorization**

This command scaffolds a project that uses Dapper for data access with an Oracle database and disables JWT authorization.

```bash
dotnet new RA.Template -n MyDapperApi --Database DapperOracle --UseAuthorization false
```

**Example 2: Project with both EF Core and Dapper for SQL Server**

The template supports multiple persistence options. This is useful if you need to use EF for some parts of your application and Dapper for performance-critical queries.

```bash
dotnet new RA.Template -n MyHybridApi --Database EfSqlServer --Database DapperSqlServer
```

**Example 3: A minimal API without Persistence or Integrations**

```bash
dotnet new RA.Template -n MyMinimalApi --Database "" --UseIntegrations false
```

### 5. Running Your New Application

1.  **Navigate to the project directory**:
    `cd YourProjectName`
2.  **Restore Dependencies**:
    `dotnet restore`
3.  **Configure Settings**: Open `src/Presentation/YourProjectName.Api/appsettings.Development.json` and update the `ConnectionStrings` section if you are using a persistence layer.
4.  **Run the application**:
    `dotnet run --project src/Presentation/YourProjectName.Api/YourProjectName.Api.csproj`
5.  **Access the API**: The application will be running on the configured port (e.g., `https://localhost:7001`). You can access the OpenAPI documentation at `https://localhost:7001/openapi-ui`.

### 6. Uninstalling the Template

To remove the template from your machine, run the following command:
```bash
dotnet new uninstall RA.CleanArchitecture.Template
```

## 🌳 RA.CleanArchitecture.Template Source Tree

```
RA.CleanArchitecture.Template/
├── .template.config/
│   └── template.json
├── src/
│   ├── Core/
│   │   ├── RaTemplate.Domain/
│   │   │   └── ... (Entities, Enums, Domain Events, etc.)
│   │   └── RaTemplate.Application/
│   │       ├── ... (Application services, CQRS handlers, etc.)
│   │       └── ApplicationServiceRegistration.cs
│   ├── Infrastructure/
│   │   ├── RaTemplate.Infrastructure/
│   │   │   └── InfrastructureServiceRegistration.cs
│   │   ├── RaTemplate.Integration/
│   │   │   └── IntegrationServiceRegistration.cs
│   │   └── RaTemplate.Persistence/
│   │       └── ... (DbContext, Repositories, Migrations for EF/Dapper)
│   └── Presentation/
│       └── RaTemplate.Api/
│           ├── Extensions/
│           │   ├── AuthorizationExtensions.cs
│           │   └── OpenApiExtensions.cs
│           ├── Endpoints/
│           │   └── ... (API endpoints)
│           ├── Program.cs
│           ├── StartupExtensions.cs
│           └── README.md
├── RA.CleanArchitecture.Template.sln
└── README.md
```
### Explanation of the Structure:
  * **`template.config/`**: This directory holds the `template.json` file, which is the heart of your .NET template, defining its parameters, conditions, and file mappings.
  * **`src/`**: This is the main source code directory.
  * **`Core/RaTemplate.Domain/`**: Contains business entities, value objects, and domain logic, with no dependencies on other layers.
  * **`Application/RaTemplate.Application/`**: This layer orchestrates the domain logic. It would contain application services, CQRS (Command Query Responsibility Segregation) handlers, DTOs (Data Transfer Objects), and interfaces for infrastructure concerns (like repositories). It depends on the Domain layer.
  * **`Infrastructure/`**: This layer contains implementations for external concerns.
    * **`RaTemplate.Infrastructure/`**: A central project for wiring up other infrastructure components.
    * **`RaTemplate.Integration/`**: Contains services for communicating with external APIs, like the HTTP client setup you have.
    * **`RaTemplate.Persistence/`**: Implements the data access logic using Entity Framework or Dapper, as chosen by the user.
  * **`Presentation/RaTemplate.Api/`**: This is the entry point of your application—the API project. It handles HTTP requests, routing, and calls into the Application layer. It depends on the Application and Infrastructure layers for dependency injection setup.

## 🧠 Summary
In summary, **RA.CleanArchitecture.Template** template provides a robust and customizable foundation for developing modern, maintainable, and scalable .NET Web APIs.

## ⚙️ Template Parameters

| Parameter | Display Name | Description | Type | Default Value | Available Choices |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `Framework` | .NET Target Framework | Select the target framework for the project. | Choice | `.NET 10` | `.NET 10` |
| `UseAuthorization` | JWT Authorization | Includes JWT-based authorization services and middleware. | Boolean | `true` | `true`, `false` |
| `UseIntegrations` | Use HTTP Client Integration? | Adds infrastructure for building and consuming external HTTP services. | Boolean | `true` | `true`, `false` |
| `OpenApiUI` | OpenApi documentation UI. | Selects the user interface for the OpenAPI (Swagger) documentation. | Choice | `scalar` | `scalar`, `swagger` |
| `Database` | Persistence Layer | Selects the data access technology. Multiple choices can be selected. | Choice | `EfSqlServer` | `EF with SQL Server`, `EF with Oracle`, `Dapper with SQL Server`, `Dapper with Oracle` |