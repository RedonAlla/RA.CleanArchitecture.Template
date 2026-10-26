---
alwaysApply: true
description: >
  Enforces Result pattern for expected failures, ProblemDetails for HTTP errors,
  and boundary-only exception handling in .NET projects.
color: red
---

# Error Handling Rules

## Result Pattern Over Exceptions

- **DO** use a `Result`/`Result<T>` pattern for expected failure paths (not found, validation, conflict).
  Rationale: Exceptions are expensive and hide control flow. Results make failure explicit in the type system.
  For `Result`/`Result<T>` package `RA.Utilities.Core.Results` and for exceptions use package `RA.Utilities.Core.Exceptions`


- **DON'T** use try-catch for flow control. If you can predict the failure, return a Result.
  Rationale: try-catch obscures the happy path and makes code harder to reason about.

- **DO** define typed error codes in your Result type:
  - `NotFound`,
  - `Validation`,
  - `Conflict`,
  - `Bad Request`,
  - `Forbidden`,
  - `Unauthorized`,
  - `Payment Required`,
  - `Timeout`
  - `Service Unavailable`
  - `Too Many Requests`.
  Rationale: Typed errors enable consistent mapping to HTTP status codes and structured logging.
  As failure `Result<T>` type use exceptions from package `RA.Utilities.Core.Exceptions` 

## HTTP Responses

- **DO** always return response of `Response<T>` from package `RA.Utilities.Api.Response`.
  Rationale: Use ErrorResultMapper to map fault result (exception) to correct Response type.

## Exception Handling Boundaries

- **DON'T** catch bare `Exception` unless at the application boundary (middleware/top-level handler).
  Rationale: Broad catches swallow bugs silently. Only the outermost layer should catch everything.

- **DON'T** catch and rethrow without adding context. Either handle it or let it propagate.
  Rationale: Catch-and-rethrow without value destroys stack traces and adds noise.

## Boundary Validation

- **DO** validate at system boundaries: API input, external service responses, file/config data.
  Rationale: Bad data should be rejected at the edge before it corrupts internal state.

- **DON'T** defensively validate inside internal/private methods.
  Rationale: Internal code should trust validated data. Double-validation adds noise without safety.