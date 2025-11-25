# 🏗️ Clean Architecture

Clean architecture is essential for organizing code in a structured way.
By adding multiple layers such as **Presentation**, **Application**, **Domain**, and **Infrastructure**, developers ensure clear separation of concerns.

Creating layers in clean architecture helps maintain a clear structure as the application grows. Each layer has its specific responsibilities, promoting better organization and readability.

Clean architecture emphasizes separation of concerns and dependency inversion to create a robust software structure. This approach ensures that each component has a distinct responsibility, promoting better maintainability.

## 🎯 Domain
The **Domain** layer of an application is crucial as it contains core classes, data transfer objects, and interfaces for necessary abstractions.

Core classes include data transfer objects and entities, which are essential for organizing the application's data structure. This organization supports better code management and scalability.

## 🗂️ Application
The application layer orchestrates the domain, handling business logic and defining use cases for features. It is essential for managing interactions between domain entities and application processes.

## 💻 Presentation
The presentation layer serves as the entry point for user interactions, facilitating communication between users and the application’s core functionalities through APIs. This design enhances usability and accessibility.

## 🛠️ Infrastructure
The infrastructure layer contains essential implementations like database interactions and third-party service calls. It also includes configurations for the Entity Framework Core.

This layer is crucial for ensuring that all external dependencies are properly managed and utilized.

# 🪾 Clean Architecture Dependency Tree

```
                        +-----------------------+
                        |   Presentation Layer  |
                        |  (API, UI, CLI, etc.) |
                        +-----------------------+
                                   |
                                   v
                        +-----------------------+
                        |   Application Layer   |
                        | (Use Cases, Services, |
                        |  Interfaces, DTOs)    |
                        +-----------------------+
                                   |
                                   v
                        +-----------------------+
                        |     Domain Layer      |
                        |  (Entities, Value     |
                        |   Objects, Rules)     |
                        +-----------------------+
                                   ^
                                   |
                        +-----------------------+
                        |  Infrastructure Layer |
                        | (Repositories,        |
                        |  External APIs,       |
                        |  EF Core, File IO)    |
                        +-----------------------+
```

# 🪢 Dependency Rules
1. **Inner layers never depend on outer layers.**
2. **Outer layers depend on abstractions defined in inner layers**, not vice versa.
3. **Domain has zero dependencies** on anything else.
4. **Application** depends only on **Domain** (for entities) and defines **interfaces**  for infrastructure concerns.
5. **Infrastructure** implements interfaces defined in **Application**.
6. **Presentation** depends on **Application** for use cases.

# 📲 Example with External API Integration

```
Controller (Presentation) 
    -> Use Case Handler (Application) 
        -> IExternalPaymentService (Application Interface) 
            -> ExternalPaymentService (Infrastructure Implementation)
                -> HttpClient (External API)

```

# 🧩 Dependency Schema with Interfaces
```
Presentation  --->  Application  --->  Domain
      |                 |                ^
      |                 |                |
      |                 +-----Interfaces-+
      |                           ^
      |                           |
      +------> Infrastructure <---+
```

* **Application** exposes **interfaces** for persistence, external services.
* **Infrastructure** implements those interfaces.
* **Presentation** uses Application’s use **cases/handlers**.
* **Domain** is completely isolated (no outward dependencies).

# 🔑 Key Principles
**✔ DIP (Dependency Inversion Principle)**: Inner layers define abstractions, outer layers implement them.<br/>
**✔ SRP (Single Responsibility Principle)**: Each layer has a clear role.<br/>
**✔ Open/Closed Principle**: You can add new infrastructure (e.g., switch DB or API) without changing core logic.