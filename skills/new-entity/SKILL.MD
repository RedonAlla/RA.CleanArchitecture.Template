---
name: new-entity
description: Create a new domain entity in the RaTemplate .NET Clean Architecture solution, including the sealed entity class, XML docs, constraints class, DbSet registration in the DbContext abstraction and implementation, and an IEntityTypeConfiguration. Use this skill whenever the user types /new-entity, or asks to add, create, scaffold, or model a new entity, domain model, table, or aggregate in this project, even if they don't say "entity" explicitly (e.g. "add a Todo", "I need a table for Invoices", "model a Payment").
---

# New Entity

Invoked with `/new-entity`. Input from the user: `$ARGUMENTS`

- If `$ARGUMENTS` contains the entity name (and optionally its properties or purpose), use it directly.
- If it is empty, or the name or properties are missing, ask the user for the entity name and its properties before creating anything.

Follow every step below, in order, each time a new entity is created.
Replace `EntityName` with the actual entity name (PascalCase, singular).

## 1. Entity class

**Path:** `/src/Core/RaTemplate.Domain/Entities/EntityName.cs`

- The class must be `sealed`.
- It must inherit from exactly one base class, chosen from the entity's purpose or its properties:

| Properties the entity has                                  | Base class            |
|------------------------------------------------------------|-----------------------|
| `Id`                                                       | `CoreEntity`          |
| `Id`, `CreatedAt`                                          | `BaseEntity`          |
| `Id`, `CreatedAt`, `IsDeleted`                             | `SoftDeleteEntity`    |
| `Id`, `CreatedAt`, `LastModifiedAt`                        | `WriteEntity`         |
| `Id`, `CreatedAt`, `LastModifiedAt`, `CreatedBy`, `LastModifiedBy` | `AuditableBaseEntity` |

- Do not redeclare properties already provided by the base class.
- If the purpose is unclear (e.g. does it need soft delete or auditing?), ask the user before choosing.
- Look at an existing entity in the folder first and match its style (namespaces, nullable annotations, constructors).
  `Entities/Event.cs` is the current reference.

### Key type and constructor

- The repo convention is `Guid` keys (see the examples under `knowledge/decisions/`). Use `BaseEntity<Guid>` unless the user's spec says otherwise.
- **The base classes do not initialize `Id`** (`RA.Utilities.Data.Entities` has a non-public parameterless constructor and `Id { get; set; }` stays `default(TKey)`). Give the entity a block-body parameterless constructor that sets it:

```csharp
public EntityName()
{
    Id = Guid.NewGuid();
}
```

- Non-nullable reference properties fail CS8618 under `TreatWarningsAsErrors`: initialize strings with `= string.Empty;` and value objects with `= null!;` (or make the property nullable).

### Audit fields and computed properties

- `CreatedAt` / `LastModifiedAt` are stamped by `BaseEntitySaveChangesInterceptor` on save — never set them manually and never redeclare them.
- Get-only computed properties (e.g. `public DateTime ExpiredAt => CreatedAt.AddDays(Duration);`) are ignored by EF Core by convention; still call `builder.Ignore(...)` in the configuration to document the intent.

### XML documentation

Every property gets XML documentation. Opening and closing tags go on separate lines:

```csharp
/// <summary>
/// The title of the todo.
/// </summary>
public string Title { get; set; } = string.Empty;
```

Never write single-line `/// <summary>text</summary>`.

- `cref` cannot resolve members inherited from the NuGet base classes: `<see cref="CreatedAt"/>` fails the build with CS1574. Use `<c>CreatedAt</c>` for inherited members.

## 2. Constraints class

**Path:** `/src/Core/RaTemplate.Domain/Constants/EntitiesConstraints/EntityNameConstraints.cs`

Constraints (max lengths, precision, etc.) must never be hardcoded in the configuration.
Put them in a `public static class EntityNameConstraints` as `const` values, each with XML docs (separate-line tags).
Example for an entity called `Todo`:

```csharp
namespace RaTemplate.Domain.Constants.EntitiesConstraints;

public static class TodoConstraints
{
    /// <summary>
    /// The maximum allowed length for the description.
    /// </summary>
    public const int DescriptionMaxLength = 300;

    /// <summary>
    /// The maximum allowed length for the title.
    /// </summary>
    public const int TitleMaxLength = 24;
}
```

- Include min-length constants from the user's spec too — validators consume them, even though only the max lengths are used by the EF configuration.
- If a legacy single-file `Constants/EntitiesConstraints.cs` (an empty `EntitiesConstraints` class) still exists, delete it first — a class and a namespace cannot share that name (CS0101).
- Skip this file if the entity has no constrained properties.

## 3. DbSet in the DbContext abstraction

**Path:** `/src/Core/RaTemplate.Application/Abstractions/Data`

Add a `DbSet<EntityName>` to the DbContext abstraction (interface).
The property name is the **plural** of the entity: `DbSet<Todo> Todos { get; }`.

If more than one DbContext abstraction exists in that folder, **ask the user which one** the DbSet belongs to before editing.
Do not guess.

## 4. DbSet in the DbContext implementation

**Path:** `/src/Infrastructure/RaTemplate.Persistence/Database`

In the DbContext class that implements the abstraction chosen in step 3, add the matching `DbSet<EntityName>` property (same plural name), using the expression-bodied `Set<EntityName>()` pattern:

```csharp
/// <summary>
/// Gets the collection of <plural name>.
/// </summary>
public DbSet<EntityName> EntityNames => Set<EntityName>();
```

## 5. IEntityTypeConfiguration

**Path:** `/src/Infrastructure/RaTemplate.Persistence/Configuration/EntityNameConfig.cs`

- Class is `internal sealed`, named `EntityName` + `Config` (e.g. `TodoConfig`), namespace `RaTemplate.Persistence.Configuration`, and implements `IEntityTypeConfiguration<EntityName>`.
- The `Config` suffix and the `RaTemplate.Persistence.Configuration` namespace are **enforced by `PersistenceTests`** — using `Configuration` as a suffix or a `Configurations` folder fails the test suite.
- Use the **Fluent API only**. No data annotations on the entity.
- Use the constants from `EntityNameConstraints` for lengths, never literal numbers.
- Configure keys, required/optional, max lengths, indexes and relationships as appropriate.
- Explicitly `builder.Ignore(e => e.ComputedProperty)` for get-only computed properties (ignored by convention, but the explicit call documents intent).

### Value converters and extensions

For these property types use the helpers from the package `RA.Utilities.Data.EntityFramework`.

| Property type   | Use                                                                 | Namespace                                          |
|-----------------|---------------------------------------------------------------------|----------------------------------------------------|
| `Currency`      | `.HasConversion<CurrencyConverter>()` on the **property** builder   | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `Email`         | `.HasConversion<EmailConverter>()` on the **property** builder      | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `SSN`           | `.HasConversion<SsnConverter>()` on the **property** builder        | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `IBAN`          | `.HasConversion<IbanConverter>()` on the **property** builder       | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `Money`         | `HasMoney()` on the **entity** builder (do **not** use a converter) | `RA.Utilities.Data.EntityFramework.Extensions`     |

Add the matching `using` for each helper you use.

`HasMoney` is an extension on `EntityTypeBuilder<T>`, **not** on `PropertyBuilder<T>`:

```csharp
builder.HasMoney(e => e.Price)
    .IsRequired();
```

It returns `ComplexPropertyBuilder<Money>` (supports `.IsRequired()`) and maps the property to two columns:
`<Prop>_Amount` (decimal) and `<Prop>_Currency` (string).
`builder.Property(e => e.Price).HasMoney()` does **not** compile — only the entity-builder form exists.

```csharp
using RA.Utilities.Data.EntityFramework.Conventions;
using RA.Utilities.Data.EntityFramework.Extensions;

internal sealed class TodoConfig : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(TodoConstraints.TitleMaxLength)
            .IsRequired();

        builder.Property(t => t.Currency)
            .HasConversion<CurrencyConverter>();

        builder.Property(t => t.Email)
            .HasConversion<EmailConverter>();

        builder.Property(t => t.Ssn)
            .HasConversion<SsnConverter>();

        builder.HasMoney(t => t.Price)
            .IsRequired();
    }
}
```

Keep the rule: `HasMoney`, never `MoneyConverter` (the converter exists in the package but `HasMoney` is the supported mapping).

### Value object API reference

Verified against the versions pinned in `Directory.Packages.props`:

- `Money(decimal amount, Currency currency)` — `decimal Amount { get; }`, `Currency Currency { get; }`,
  static `PositiveMoney(decimal, Currency)`, `Add`/`Subtract`, equality operators.
- `Currency(string value)` — validates ISO 4217 (3 characters, trimmed, upper-cased), `string Value { get; }`,
  implicit conversion to `string`.
- `Email(string)`, `SSN(string)`, `IBAN(string)` — same constructor-from-string pattern with a `Value` property.

## 6. Verifying package APIs fast

The public API of every `RA.Utilities.*` package is documented in its XML file in the local NuGet cache —
grep it instead of decompiling or writing reflection probes:

```bash
grep -o 'member name="[^"]*"' \
  ~/.nuget/packages/<package-id-lowercased>/<version>/lib/net10.0/*.xml \
  | grep -i <TypeOrMemberName>
```

- Package versions are pinned in `Directory.Packages.props`; the cache folder is the package id in lowercase.
- `BaseEntitySaveChangesInterceptor` lives in `RA.Utilities.Data.EntityFramework.Interceptors` and stamps
  `CreatedAt`/`LastModifiedAt`/`IsDeleted`/audit fields on the `CoreEntity<TKey>` hierarchy on save.
- Key/value-generation needs no configuration: a property named `Id` is the key by convention, and `Guid`
  keys are generated on add (`ValueGeneratedOnAdd`).

## Final checklist

Before reporting done, confirm:

- [ ] Entity is `sealed`, in `/src/Core/RaTemplate.Domain/Entities`, `BaseEntity<Guid>` unless told otherwise, with a parameterless constructor that sets `Id`
- [ ] Every entity property has multi-line XML docs; inherited members referenced with `<c>...</c>`, never `cref`
- [ ] `EntityNameConstraints` exists (if needed) and no magic numbers remain in the configuration
- [ ] `DbSet` (plural) added to the abstraction (`IRaTemplateSqliteDbContext`) and to the implementing DbContext
- [ ] `EntityNameConfig` is `internal sealed` in `RaTemplate.Persistence.Configuration`, Fluent API only, `HasMoney` on the entity builder
- [ ] Legacy `Constants/EntitiesConstraints.cs` placeholder removed if it still existed
- [ ] Solution builds (`dotnet build`) if the environment allows it

Do not create an EF migration unless the user asks for one.