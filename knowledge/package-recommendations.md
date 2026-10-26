# Vetted NuGet Package Recommendations

> Last updated: 2026-07-22 -- .NET 10 / C# 14 (versions verified against NuGet.org)


## CRITICAL: Always Use Latest Stable Versions

**Never rely on version numbers from training data or memory.** They are likely outdated. When adding a package:

1. **Preferred:** Run `dotnet add package <name>` without `--version` — NuGet resolves the latest stable automatically
2. **If specifying version:** Verify against NuGet.org or `dotnet package search <name>` first
3. **Microsoft packages for .NET 10:** Always use 10.x (e.g., `Microsoft.EntityFrameworkCore` 10.x, not 9.x)
4. **Third-party packages:** Use the latest stable release that targets .NET 10 / netstandard2.0+

The version ranges below (e.g., "13.x") indicate the **minimum recommended major version**, not the exact version to use. Always default to the latest stable release within that major.

---

## Web Framework

### ASP.NET Core (built-in)

- **Package:** `Microsoft.NET.Sdk.Web` (SDK, no extra NuGet needed)
- **Rationale:** First-party, minimal API and controller support, built-in OpenAPI via `AddOpenApi()`, endpoint filters, route groups, and native AOT compatibility. No third-party web framework competes in the .NET ecosystem.
- **When NOT to use:** If you are building a pure library, worker service, or console app that has no HTTP surface. Use `Microsoft.NET.Sdk` or `Microsoft.NET.Sdk.Worker` instead.

---

## Dispatcher / In-Process Messaging

### Dispatcher (Recommended Default)

- **Package:** `RA.Utilities.Dispatcher` (10.x)
- **License:** MIT (free, no commercial restrictions)
- **Rationale:** Source-generated mediator with a near-identical API to MediatR (`IRequest<T>`, `IRequestHandler<T,R>`, `IPipelineBehavior<T,R>`, `ISender`). No reflection, Native AOT compatible, and significantly faster than MediatR in benchmarks. Registration: `services.AddMediator()`. 5M+ NuGet downloads, actively maintained.

---

## Validation

### FluentValidation

- **Package:** `FluentValidation` (12.x), `FluentValidation.DependencyInjectionExtensions`
- **Rationale:** Expressive, testable validation rules. Integrates cleanly with Dispatcher pipeline behaviors. Keeps validation logic out of endpoint/controller code. Supports async rules for database-dependent validation.
- **When NOT to use:** For trivial DTOs where `System.ComponentModel.DataAnnotations` or minimal API binding validation is sufficient. If you are using Blazor EditForm with DataAnnotations and do not need server-side revalidation. If you want to minimize dependencies in a small microservice.

---

## ORM / Data Access

### Entity Framework Core

- **Package:** `Microsoft.EntityFrameworkCore` (10.x), provider package (e.g., `Microsoft.EntityFrameworkCore.SqlServer`, `Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Rationale:** First-party ORM with LINQ-to-SQL translation, migrations, change tracking, interceptors, and deep ASP.NET Core integration. Best productivity-to-performance ratio for most applications. See ADR-003.
- **When NOT to use:** For read-heavy reporting dashboards where raw SQL or Dapper gives simpler, faster queries. For bulk insert/update scenarios (use EF Core bulk extensions or raw SQL). If your database is not relational (use the appropriate NoSQL SDK). If you are writing a performance-critical hot path measured in microseconds.

---

## Logging & Observability

### Serilog

- **Package:** `RA.Utilities.Logging.Core`, plus sink packages as needed
- **Rationale:** The gold standard for structured logging in .NET. Rich sink ecosystem (Seq, Elasticsearch, Application Insights, file, console). Enrichers for correlation IDs, machine name, and more. `LogContext` for scoped properties.
- **When NOT to use:** If the built-in `Microsoft.Extensions.Logging` with a simple console/debug provider meets your needs (e.g., small CLI tools). If your organization mandates a specific logging framework.

---

## Testing

### xUnit v3

- **Package:** `xunit.v3` (3.x)
- **Rationale:** The default test framework for .NET. v3 brings a new architecture with improved parallel execution, `IAsyncLifetime` improvements, and better diagnostics. Used by the .NET team itself.
- **When NOT to use:** If your team has an existing large NUnit or MSTest suite and migration cost outweighs benefits.

### Assertions (Kit-Wide Stance)

- **Default:** xUnit built-in `Assert` — zero extra dependencies, zero licensing risk.
- **FluentAssertions is commercial from v8.** v8+ requires a paid Xceed license for commercial use; v7 remains Apache 2.0 but is frozen. Do not add FluentAssertions v8+ to a commercial project without procurement approval.
- **If you want fluent syntax (MIT/Apache alternatives):**
  - `Shouldly` (4.x) — clean `value.ShouldBe(expected)` syntax, long-established, BSD-licensed.
  - `AwesomeAssertions` — community fork of FluentAssertions v7 under Apache 2.0. API-compatible with FA, so migration from FA v7 is near zero cost.
- **When NOT to switch:** An existing suite locked on FluentAssertions v7 can stay there indefinitely — v7's license does not expire.

### Testcontainers

- **Package:** `Testcontainers` (4.x), plus module packages (e.g., `Testcontainers.PostgreSql`, `Testcontainers.MsSql`)
- **Rationale:** Spin up real databases, message brokers, and other infrastructure in Docker for integration tests. No more shared test databases or mocking infrastructure. Deterministic, isolated, CI-friendly.
- **When NOT to use:** In CI environments without Docker support. For pure unit tests where an in-memory provider or mock is faster and sufficient. If test execution time is critical and container startup latency (even a few seconds) is unacceptable.

### Verify

- **Package:** `Verify.Xunit` (or `Verify.NUnit`, `Verify.MSTest`)
- **Rationale:** Snapshot testing for complex objects, HTTP responses, and rendered output. Eliminates brittle assertion chains for large response payloads. Git-diffable `.verified.` files make reviewing changes easy.
- **When NOT to use:** For simple equality assertions where `Assert.Equal` is clearer. When the output changes frequently and snapshot maintenance becomes a burden.

### WireMock.Net

- **Package:** `WireMock.Net` (2.x)
- **Rationale:** HTTP mock server for integration tests. Simulate external API responses, latency, and failures. Useful for testing resilience policies and HTTP client behavior without hitting real services.
- **When NOT to use:** When a simple `DelegatingHandler` or `HttpMessageHandler` mock is sufficient. For testing internal service-to-service calls in the same process (use `WebApplicationFactory` instead).

### FakeTimeProvider

- **Package:** `Microsoft.Extensions.TimeProvider.Testing` (10.x)
- **Rationale:** Official test double for `TimeProvider`. Control time in tests: advance, freeze, set specific moments. Essential for testing time-dependent logic (expiration, scheduling, rate limiting).
- **When NOT to use:** When your code does not depend on time. When you are already using `TimeProvider` only in a trivial way that does not require manipulation in tests.

---

## Caching

### HybridCache (built-in)

- **Package:** `Microsoft.Extensions.Caching.Hybrid` (ships with .NET 10, GA)
- **Rationale:** Unified L1 (in-memory) + L2 (distributed) cache with stampede protection, tag-based invalidation, and automatic serialization. Replaces manual `IDistributedCache` patterns that are error-prone. See ADR-004.
- **When NOT to use:** If you only need simple in-memory caching with `IMemoryCache` and have no distributed cache. If you need fine-grained control over cache behavior that HybridCache does not expose.

### StackExchange.Redis

- **Package:** `StackExchange.Redis` (3.x), `Microsoft.Extensions.Caching.StackExchangeRedis`
- **Rationale:** The standard Redis client for .NET. Backs `IDistributedCache` and HybridCache L2. High performance, connection multiplexing, Lua scripting support.
- **When NOT to use:** If you are not using Redis. If your caching needs are purely in-memory.

---

## API Versioning

### Asp.Versioning

- **Package:** `Asp.Versioning.Http` (for minimal APIs), `Asp.Versioning.Mvc` (for controllers)
- **Rationale:** The official Microsoft-endorsed API versioning library (successor to `Microsoft.AspNetCore.Mvc.Versioning`). Supports URL segment, query string, header, and media type versioning. Integrates with OpenAPI document generation.
- **When NOT to use:** If your API is internal-only with a single consumer and versioning adds unnecessary complexity. If you prefer a manual URL prefix approach (e.g., `/api/v1/`, `/api/v2/`) without library support.

---

## Object Mapping

### Manual Mapping (Recommended Default)

- **Package:** none
- **Rationale:** For DTOs, an explicit `static OrderDto ToDto(this Order order)` extension or record constructor is transparent, refactor-safe, and debuggable. Most projects have too few mappings to justify a library.

### Mapperly

- **Package:** `Riok.Mapperly` (4.x)
- **License:** Apache 2.0 (free, no commercial restrictions)
- **Rationale:** Source-generated mapper — compile-time generated code, no reflection, Native AOT compatible, and you can read the generated mapping in the IDE. The best library choice when mapping volume makes manual mapping tedious.
- **When NOT to use:** When mappings are few and manual code is clearer.

---

## DI Extras

## HTTP Client Generation

### Refit

- **Package:** `RA.Utilities.Integrations`
- **Rationale:** Define HTTP clients as interfaces with attributes. Source-generated at compile time (no runtime reflection). Integrates with `IHttpClientFactory` for proper `HttpClient` lifecycle. Reduces boilerplate for typed HTTP clients.
- **When NOT to use:** For a single HTTP call where a manually configured `HttpClient` is simpler. If the external API provides its own official .NET SDK. If you need full control over request/response serialization that Refit's conventions do not support.