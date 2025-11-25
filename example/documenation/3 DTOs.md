# 🧱 Where to Place DTOs in Clean Architecture

The placement of Data Transfer Objects (DTOs) depends on their purpose and how they're used across layers. Here's a breakdown to help you decide:

## Key Principles for DTO Placement

1. **Dependency Direction:** DTOs should only be referenced by outer layers, never inner layers
2. **Single Responsibility:** Each DTO should serve one specific communication purpose
3. **Layer Isolation:** Domain layer should never know about DTOs
4. **Explicit Conversion:** Use mappers/converters between different DTO types

## When to Use Which DTO Type

| DTO Type	           | Layer                |	Purpose                        |
| -------------------- | -------------------- | ------------------------------ |
| **Request DTO**      |	Interface Adapters	| HTTP input validation.         |
| **Response DTO**     |	Interface Adapters	| HTTP output formatting.        |
| **Use Case Input**   |	Application	        | Use case parameter validation. |
| **Use Case Output**  |	Application	        | Complex use case results.      |
| **Repository Query** |	Application	        | Database query parameters.     |
| **External API DTO** |	Infrastructure	    | External service communication |

## Anti-Patterns to Avoid
* ❌  **Domain entities as DTOs** - Entities contain business logic, DTOs are data containers
* ❌ **DTOs crossing layer boundaries inward** - DTOs should flow outward only
* ❌ **God DTOs** - One DTO used for multiple purposes
* ❌ **Business logic in DTOs** - Keep DTOs simple data structures

## 1. Application Layer
**✅ Recommended for most DTOs**

* This layer orchestrates use cases and interacts with both the domain and presentation layers.
* ⁠DTOs here are used to carry data between the domain and external layers (e.g., controllers, APIs).
* ⁠They help isolate the domain model from external concerns.

> Example: ⁠ 
> `UserDto` ⁠ used in a ⁠ `CreateUserUseCase` ⁠ to receive input from a controller.

## 2. Presentation Layer
**✅ For View Models or UI-specific DTOs**
* These DTOs are tailored for rendering views or handling user input.
* ⁠They often differ from domain models in structure or naming.

> Example: ⁠ 
> `UserViewModel`⁠ used in an MVC controller or frontend API response.

## 3. Domain Layer
**🚫 Avoid placing DTOs here**

* ⁠The domain layer should remain pure and unaffected by infrastructure or presentation concerns.
* ⁠DTOs are not part of the core business logic.

## Directory Structure

```
src/
├── application/
│   ├── use-cases/
│   │   ├── create-user/
│   │   │   ├── CreateUserInput.ts      # Use Case Input DTO
│   │   │   ├── CreateUserOutput.ts     # Use Case Output DTO (if needed)
│   │   │   └── CreateUserUseCase.ts
│   │   └── get-user-report/
│   │       ├── UserReportOutput.ts     # Complex output DTO
│   │       └── GetUserReportUseCase.ts
│   └── interfaces/
│       ├── repositories/
│       │   ├── UserRepository.ts
│       │   └── UserSearchCriteria.ts   # Repository query DTO
│       └── services/
│           └── EmailService.ts
├── domain/
│   └── entities/
│       └── User.ts                     # Domain Entity (not a DTO!)
└── infrastructure/
    ├── controllers/
    │   ├── requests/
    │   │   ├── CreateUserRequest.ts    # HTTP Request DTO
    │   │   └── UpdateUserRequest.ts
    │   ├── responses/
    │   │   ├── UserResponse.ts         # HTTP Response DTO
    │   │   └── ErrorResponse.ts
    │   └── UsersController.ts
    ├── persistence/
    │   └── repositories/
    │       └── UserRepositoryImpl.ts   # Implements application interface
    └── integrations/
        └── payment-service/
            ├── PaymentRequestDto.ts    # External API Request DTO
            ├── PaymentResponseDto.ts   # External API Response DTO
            └── PaymentServiceClient.ts
```