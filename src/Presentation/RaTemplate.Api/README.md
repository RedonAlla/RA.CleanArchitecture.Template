# RA.CleanArchitecture.Template

**RA.Clean.Architecture.Template** is a robust.NET solution template designed to streamline development using Clean Architecture principles.
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
│   └── Web/
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
  * **`Web/RaTemplate.Api/`**: This is the entry point of your application—the API project. It handles HTTP requests, routing, and calls into the Application layer. It depends on the Application and Infrastructure layers for dependency injection setup.

## 🧠 Summary
In summary, **RA. Clean Architecture Template** template provides a robust and customizable foundation for developing modern, maintainable, and scalable .NET Web APIs.