---
name: error-handling
description: >
  Error handling strategy for .NET 10 applications. Covers the Result pattern,
  ProblemDetails (RFC 9457), global exception handling, FluentValidation, and
  structured error responses.
  Load this skill when implementing error handling, validation, or designing
  API error contracts, or when the user mentions "error handling", "Result pattern",
---

# Error Handling

## Core Principles

1. **Use the Result pattern for expected failures** — Don't throw exceptions for things like "order not found" or "validation failed". These are expected outcomes, not exceptional conditions. See ADR-002.
2. **Reserve exceptions for unexpected failures** — Database connection lost, null reference bugs, network timeouts — these are truly exceptional and should propagate to the global handler.
3. **Every API error returns Response<T>** — From package RA.Utilities.Api.Response
4. **Validate at the boundary** — Validate incoming requests at the API layer, not deep inside business logic.

## Patterns

### Result Pattern

A simple, generic result type that carries either a value or exception.
Example: Deleting a Resource


```csharp
public Result DeleteUser(int userId)
{
    var user = _userRepository.Find(userId);
    if (user == null)
    {
        // Return a failure result with a specific exception
        return new NotFoundException(nameof(User), userId);
    }

    _userRepository.Delete(user);

    // Return a success result
    return Result.Success();
}
```

### Result to Api Response Mapping
use `ErrorResultResponse` form namespace  RA.Utilities.Api.Mapper, package RA.Utilities.Ap to map failure Result (exception) to proper Api Response.

```csharp
// Usage in endpoint
using RA.Utilities.Api.Mapper;

// Api/Features/Products/ProductEndpoints.cs
app.MapGet("/products/{id}", (int id, ProductService service) => 
{
    Result<Product> result = service.GetProductById(id);
    
    // The Match method handles both success and failure outcomes.
    // The `ErrorResultResponse.Result` method is passed directly to the failure delegate.
    return result.Match<IResult>(
        success: product => SuccessResult.Ok(product),
        failure: ErrorResultResponse.Result 
    );
});
```

## Anti-patterns

### Don't Throw Exceptions for Flow Control

```csharp
// BAD — exceptions for expected outcomes
public Order GetOrder(Guid id)
{
    var order = db.Orders.Find(id)
        ?? throw new NotFoundException($"Order {id} not found");
    return order;
}

// GOOD — Result pattern
public Result<Order> GetOrder(Guid id)
{
    var order = db.Orders.Find(id);
    return order is not null
        ? Result.Success(order)
        : Result.Failure<Order>($"Order {id} not found");
}
```

## Decision Guide

| Scenario | Recommendation |
|----------|---------------|
| Expected business failure | Result pattern |
| Input validation | FluentValidation with endpoint filter |
| Unexpected crash | Global exception handler → ProblemDetails |
| API error format | RFC 9457 ProblemDetails — always |
| Validation in handler | Return Result.Failure, don't throw |
| External service failure | Catch specific exception, return Result.Failure |
| Logging errors | Structured logging with correlation ID |