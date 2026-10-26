# What's New in .NET 10 and C# 14

> Last updated: 2026-05-22 — .NET 10 GA / .NET 11 preview 6

## C# 14 Language Features

### Extension Members
Extension methods evolve into full extension members — properties, static methods, and more.

```csharp
public extension StringExtensions for string
{
    public bool IsNullOrEmpty => string.IsNullOrEmpty(this);
    public string Truncate(int maxLength) => Length <= maxLength ? this : this[..maxLength] + "...";
}
```

### The `field` Keyword
Access the compiler-generated backing field in property accessors.

```csharp
public string Name
{
    get => field;
    set => field = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
}
```

### `allows ref struct` in Generics
Generic type parameters can now accept `ref struct` types, enabling `Span<T>` in generic contexts.

```csharp
public T Parse<T>(ReadOnlySpan<char> input) where T : ISpanParsable<T>
    => T.Parse(input, null);
```

### `params` Collections
`params` now works with any collection type, not just arrays.

```csharp
public void Log(params ReadOnlySpan<string> messages) { }
public void Add(params IEnumerable<int> values) { }
```

### Improved Pattern Matching
Extended property patterns and dictionary patterns.

### Partial Properties and Indexers
Source generators can now use partial properties.

## .NET 10 Runtime & Libraries

### ASP.NET Core
- **Built-in OpenAPI** — `builder.Services.AddOpenApi()` replaces Swashbuckle for most scenarios
- **Improved minimal API binding** — Better `[AsParameters]` support, complex type binding
- **HybridCache GA** — Graduated from preview; the default caching abstraction
- **Blazor enhancements** — Improved SSR, navigation, enhanced form handling

### Entity Framework Core 10
- **LINQ improvements** — Better query translation for complex expressions
- **ExecuteUpdateAsync/ExecuteDeleteAsync** refinements
- **Improved migrations** — Better handling of complex schema changes
- **Enhanced value converters** — Less ceremony for common patterns

### BCL (Base Class Library)
- **`TimeProvider`** — Standard abstraction for time (GA since .NET 8, now widely adopted)
- **`FrozenDictionary<K,V>` / `FrozenSet<T>`** — Immutable collections optimized for read-heavy workloads
- **`SearchValues<T>`** — Optimized searching within sets of values
- **`CompositeFormat`** — Pre-parsed format strings for high-perf logging
- **`Lock` type** — Dedicated lock type replacing `object` locks (since .NET 9)

### Performance
- **Dynamic PGO on by default** — Profile-guided optimization automatically tunes hot paths
- **Native AOT improvements** — Better trimming, more libraries compatible
- **Arm64 optimizations** — Continued investment in ARM performance

### Container & Deployment
- **Smaller container images** — `mcr.microsoft.com/dotnet/aspnet:10.0` with reduced layers
- **Non-root by default** — Container images run as non-root user
- **Aspire 13.x** — Rebranded polyglot "Aspire" (aspire.dev): first-class Python/JS/TypeScript AppHosts, `aspire` CLI built for AI coding agents, single-file AppHost via `#:package` directives

## Key NuGet Ecosystem Updates

| Package | Version | Notable Changes |
|---------|---------|----------------|
| RA.Utilities.Api | 10.x | Collection of essential utilities for building robust and consistent ASP.NET Core APIs. This package includes endpoint registration helpers, standardized response models, and exception handling middleware to streamline development and promote best practices. |
| RA.Utilities.Api.Response | 10.x | Provides a standardized set of models for creating consistent API responses, including generic wrappers for success, error, and validation failure scenarios. |
| RA.Utilities.Application.Validation | 10.x | FluentValidation for validating incoming requests. It aggregates multiple validators asynchronously and converts FluentValidation failures into structured BadRequestException/ValidationError results. |
| RA.Utilities.Dispatcher | 10.x | Kit default mediator — source-generated, |
| RA.Utilities.Core.Exceptions | 10.x | Kit of standardized, semantic exceptions for use in the RaTemplate ecosystem. |
| RA.Utilities.Core.Results | 10.x | A lightweight .NET library designed to enhance error handling by providing a functional `Result` type. This approach allows for a clear and explicit representation of success or failure, making your application's control flow easier to follow and maintain. |
| RA.Utilities.Data.Entities | 10.x | Provides strongly typed, generic base classes for domain and data entities in .NET and EF Core. |
| RA.Utilities.Integrations | 10.x | A utility library to simplify and standardize HTTP client calls in .NET applications. It builds upon `IHttpClientFactory` to provide a structured way to configure and use typed HTTP clients for various integrations. |
| FluentValidation | 12.x | .NET 10 support, better minimal API integration |
| RA.Utilities.Logging.Core | 10.x | Provides a set of opinionated helpers and configurations for setting up structured logging with Serilog in .NET applications. This package simplifies the integration of common sinks (Console, File), enrichers (Exceptions, Sensitive Data), and performance features like asynchronous logging, enabling a robust and consistent logging strategy out of the box |
| xUnit (`xunit.v3`) | 3.x | New architecture, `IAsyncLifetime` improvements |
| Testcontainers | 4.x | Faster startup, more container modules |


## .NET 11 Preview Watch (GA: November 10, 2026 — STS)

> Preview 6 shipped 2026-07-15. .NET 10 remains the latest GA and the kit's target until .NET 11 ships. Do not generate net11.0 / C# 15 code unless the user explicitly targets previews.

### C# 15 (preview, ships with .NET 11)

- **Union types** — `public union Pet(Cat, Dog, Bird);` with implicit conversions and exhaustive `switch`
- **Closed hierarchies** — `closed` modifier fixes direct descendants at compile time; exhaustive switching without a default arm
- **Collection expression arguments** — `[with(capacity: 16), .. values]` passes constructor args (capacity, comparers) inside collection expressions
- **Extension indexers** — indexers in `extension` blocks, completing the C# 14 extension-members story
- **Memory-safety model (first step)** — pointer declaration, `&`, `fixed`, and `sizeof` no longer require `unsafe` under preview LangVersion; dereferencing still does

### Platform

- **EF Core 11** — preview 6 available alongside runtime previews
- **Container images 15–33% smaller** — e.g., Alpine image down ~124 MB (30.9%), Azure Linux down 32.7%

## Migration Notes (.NET 9 to .NET 10)

1. Update `TargetFramework` to `net10.0`
2. Update `LangVersion` to `14` (or `latest`)
3. Review `breaking-changes.md` for specific breaking changes
4. HybridCache is now GA — migrate from manual `IDistributedCache` patterns
5. Built-in OpenAPI may replace your Swashbuckle dependency