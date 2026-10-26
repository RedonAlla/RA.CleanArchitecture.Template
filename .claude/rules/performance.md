---
alwaysApply: true
description: >
  Enforces performance best practices for .NET applications including async
  patterns, caching, resource management, and hot-path optimizations.
color: green
---

# Performance Rules

## Async Patterns

- **Always propagate `CancellationToken` through the call chain.** Dropped tokens mean cancelled requests continue burning server resources.

```csharp
// DO
public Task<Order?> GetOrderAsync(Guid id, CancellationToken ct) =>
    db.Orders.FirstOrDefaultAsync(o => o.Id == id, ct);

// DON'T — token silently ignored
public Task<Order?> GetOrderAsync(Guid id, CancellationToken ct) =>
    db.Orders.FirstOrDefaultAsync(o => o.Id == id);
```

- **Async all the way — no `.Result` or `.Wait()`.** Synchronously blocking on async code causes thread pool starvation and deadlocks. The only acceptable sync-over-async location is `Program.cs` top-level statements.

## Resource Management

## Caching

- **`HybridCache` over `IMemoryCache` / `IDistributedCache`.** `HybridCache` provides stampede protection, L1+L2 caching, and tag-based invalidation out of the box.

```csharp
// DO
var order = await cache.GetOrCreateAsync(
    $"order:{id}",
    async ct => await db.Orders.FindAsync([id], ct),
    cancellationToken: ct);
```

## EF Core and Hot Paths

- **Use compiled queries for hot-path EF Core queries.** Compiled queries skip expression tree translation on every call.

```csharp
private static readonly Func<AppDbContext, Guid, CancellationToken, Task<Order?>> GetById =
    EF.CompileAsyncQuery((AppDbContext db, Guid id, CancellationToken ct) =>
        db.Orders.FirstOrDefault(o => o.Id == id));
```

- **Prefer `ValueTask<T>` over `Task<T>` for high-throughput paths that often complete synchronously.** Avoids `Task` allocation when the result is already available. Use `Task` for general-purpose code where simplicity matters more.