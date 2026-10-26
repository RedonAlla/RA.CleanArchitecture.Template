---
alwaysApply: true
description: >
    Enforces modern C# 14 coding conventions, naming standards, and file organization for all .NET code in this repository.
    Enforces .editorconfig rules (formatting, naming, style, diagnostics) plus file organization and type-declaration conventions on every C# write or edit. 
color: pink
---

# C# Coding Standards (Merged)

This file is **always active** when writing or editing C# code in this repository. Every code change must comply with the rules below.
Treat violations as build errors: the project has `TreatWarningsAsErrors` enabled and `IDE0055` (formatting) is set to `error`.

**Precedence:** where a rule in this file conflicts with another guideline, the **.editorconfig rules (Parts 1 and 2)** always win.
Part 3 only adds rules that do not contradict them.

---

# Part 1: Formatting, Syntax and Style (.editorconfig rules)

## Indentation & Whitespace
```
4 spaces (no tabs)
CRLF line endings
Final newline at end of file
No multiple consecutive blank lines
```

## Namespaces & Usings
```csharp
// ✅ File-scoped namespace, usings outside
using System;
using RA.Utilities.Core.Constants;

namespace RA.Utilities.Core.Exceptions;

public class MyException : RaBaseException
{
```

```csharp
// ❌ Block-scoped namespace, usings inside
namespace RA.Utilities.Core.Exceptions
{
    using System;
```

## Braces: Allman style (always)
```csharp
// ✅
if (condition)
{
    DoSomething();
}
else
{
    DoOther();
}

// ❌
if (condition) {
    DoSomething();
}
```

## Implicit `var`: only when the type IS apparent
```csharp
// ✅ var: type is obvious from the right side
var product = new Product();
var users = new List<User>();

// ✅ explicit: built-in types, or type not obvious
int count = 42;
string name = "hello";
ProductDto dto = _mapper.Map<ProductDto>(product);
HttpResponseMessage response = await _client.GetAsync(url);
```

## No `this.` qualification
```csharp
// ✅
_property = value;

// ❌
this._property = value;
```

## Predefined types (BCL aliases)
```csharp
// ✅
string, int, bool, long, double, object

// ❌
String, Int32, Boolean, Int64, Double, Object
```

## Expression-bodied members
```csharp
// ✅ operators, properties, indexers, accessors, lambdas, local functions
public static implicit operator string(ResponseType type) => type.Value;
public string Value { get; init; }
public override string ToString() => Value;
int Sum(int a, int b) => a + b;

// ❌ methods and constructors: always use block bodies
public RaBaseException(int code, ResponseType type, string message)
    : base(message)                    // block body, not =>
{
    ErrorCode = code;
}
```

## Pattern matching & null checks
```csharp
// ✅ pattern matching
if (product is null) { }
if (result is not null) { }
var label = obj switch { { Value: "X" } => "Y", _ => "Z" };

// ✅ null propagation & coalescing
var name = obj?.Name ?? "default";

// ❌
if (product == null) { }
if (result != null) { }
```

Prefer pattern matching and switch expressions over if-else chains:

```csharp
// ✅
var label = status switch
{
    OrderStatus.Pending => "Awaiting payment",
    OrderStatus.Shipped => "On the way",
    _ => "Unknown"
};
```

## Object/collection initializers
Collections are created with **initializers**, not collection expressions (`[ ... ]`).
```csharp
// ✅
var errors = new[] { new ValidationError("msg") { PropertyName = "X" } };
var list = new List<string> { "a", "b" };
var obj = new Product { Name = "X", Price = 9.99m };

// ❌
var errors = new ValidationError[1];
errors[0] = new ValidationError("msg");
errors[0].PropertyName = "X";

// ❌ collection expressions are not used in this repository
List<int> ids = [1, 2, 3];
int[] arr = [4, 5, 6];
```

## Simple defaults & compound assignment
```csharp
// ✅
int x = 0;              // not default(int)
string s = null;        // not default
x += 5;                 // compound assignment

// ❌
int x = default(int);
x = x + 5;
```

## `using` statements: simple form
```csharp
// ✅
using var scope = _logger.BeginScope("{Id}", id);

// ❌
using (var scope = _logger.BeginScope("{Id}", id)) { }
```

## Spacing
```csharp
// ✅
(int)x                  // no space after cast
Method(arg1, arg2)      // space after comma, no space before (
x + y                   // space around binary operators
obj.Property            // no space around dot

// ❌
(int) x                 // space after cast
Method( arg1,arg2 )     // space before (, missing space after comma
x+y                     // no space around operator
```

## Modifier order
```csharp
// ✅ access modifiers first, then static, then readonly/const
private static readonly JsonSerializerOptions JsonOptions = new();
public const int Success = 200;
protected internal ResponseType(string value) { }

// ❌
static private readonly JsonSerializerOptions JsonOptions;
readonly public const int Success;
```

## Naming
```csharp
// ✅
public interface IRepository { }           // I prefix
public class ProductService { }            // PascalCase types
public string FirstName { get; set; }      // PascalCase properties
public async Task<int> GetCountAsync() { } // PascalCase methods, Async suffix

// ❌
public interface Repository { }
public class product_service { }
public string firstName { get; set; }
```

- **PascalCase** for public members, types, namespaces and methods.
- **camelCase** for local variables and parameters.
- **`_camelCase`** for private fields (underscore prefix is required).
- **Async suffix on all async methods:** `GetOrderAsync`, not `GetOrder`, for methods returning `Task` or `ValueTask`.

## XML Doc Comments
```csharp
// ✅ every public member gets XML doc
/// <summary>
/// Represents the HTTP status code 200 (OK).
/// </summary>
public const int Success = 200;

/// <summary>
/// Initializes a new instance of the <see cref="NotFoundException"/> class.
/// </summary>
/// <param name="entity">The entity type that was not found.</param>
/// <param name="value">The identifier used to search.</param>
public NotFoundException(string entity, object value) { }
```

---

# Part 2: Type Declarations

## Constructors and dependency injection

- **Primary constructors for DI injection.** Eliminates boilerplate field assignments and `_field = field` ceremony.

```csharp
// DO
public sealed class OrderService(IDbContext db, TimeProvider clock) { }

// DON'T
public class OrderService
{
    private readonly IDbContext _db;
    public OrderService(IDbContext db) { _db = db; }
}
```

## Records for DTOs and value objects
Records give immutability, value equality and `with` expressions. Document the positional parameters with `<param>` tags on the type.

```csharp
/// <summary>
/// Request to create an order.
/// </summary>
/// <param name="ProductId">The product identifier.</param>
/// <param name="Quantity">The requested quantity.</param>
public sealed record CreateOrderRequest(string ProductId, int Quantity);
```

## `sealed` and accessibility
- **`sealed` on classes not designed for inheritance.** It lets the JIT devirtualize calls and communicates intent.
- **`internal` by default, `public` only when needed.** Types that are part of a library's public API (for example, NuGet packages) are `public`; everything else stays `internal`.

---

# Part 3: File Organization

- **File-scoped namespaces always** (see Part 1).
- **One type per file.** The file name must match the type name exactly (`OrderService.cs` contains `OrderService`).
- **Order members:** constants, fields, constructors, properties, public methods, private methods.

---

# Pre-submit Checklist

Before writing or editing any `.cs` file, verify:

1. File-scoped namespace with usings outside
2. 4-space indentation (no tabs), CRLF, final newline
3. Allman braces (newline before `{`)
4. No `this.` prefix
5. BCL type aliases (`string`, `int`, `bool`)
6. `var` only when the type is apparent from the right side
7. `is null` / `is not null` (not `== null`)
8. Expression-bodied: properties, operators, accessors ✅ | methods, constructors ❌
9. Pattern matching and switch expressions where applicable
10. Object/collection **initializers** used (no collection expressions)
11. `new()` / simple defaults used
12. Compound assignment used
13. Simple `using` statements (no braces)
14. Correct spacing: no space after cast, space around binary ops, no space before `(`
15. Correct modifier order: `public static readonly`, `private const`
16. PascalCase naming, `I` prefix for interfaces, `Async` suffix on async methods, `_camelCase` private fields
17. XML doc comments on all public members (class, methods, properties, constants, constructors)
18. Classic block-body constructors (no primary constructors)
19. `sealed` on classes not meant for inheritance; `internal` unless public API
20. One type per file, members in the agreed order
