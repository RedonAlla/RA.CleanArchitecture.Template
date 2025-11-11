# RA. Clean Architecture Template

**RA. Clean Architecture Template** is a robust.NET solution template designed to streamline development using Clean Architecture principles.
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
•⁠  ⁠Developers looking for a clean, opinionated.NET template
•⁠  ⁠Teams adopting Clean Architecture for long-term scalability
•⁠  ⁠Projects that require separation of concerns and testability from day one

The project structure is modular. For instance, if you disable `UseIntegrations`, the entire `RaTemplate.Integration` project is excluded. Similarly, the persistence and authorization components are only included if you select them, keeping the final solution clean and free of unused code.

## 🧠 Summary
In summary, **RA. Clean Architecture Template**  template provides a robust and customizable foundation for developing modern, maintainable, and scalable .NET Web APIs.