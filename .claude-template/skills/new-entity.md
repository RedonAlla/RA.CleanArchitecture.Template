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

### XML documentation

Every property gets XML documentation. Opening and closing tags go on separate lines:

```csharp
/// <summary>
/// The title of the todo.
/// </summary>
public string Title { get; set; } = string.Empty;
```

Never write single-line `/// <summary>text</summary>`.

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

Only create constants the configuration actually uses.
Skip this file if the entity has no constrained properties.

## 3. DbSet in the DbContext abstraction

**Path:** `/src/Core/RaTemplate.Application/Abstractions/Data`

Add a `DbSet<EntityName>` to the DbContext abstraction (interface).
The property name is the **plural** of the entity: `DbSet<Todo> Todos { get; }`.

If more than one DbContext abstraction exists in that folder, **ask the user which one** the DbSet belongs to before editing.
Do not guess.

## 4. DbSet in the DbContext implementation

**Path:** `/src/Core/RaTemplate.Persistence/Database`

In the DbContext class that implements the abstraction chosen in step 3, add the matching `DbSet<EntityName>` property (same plural name), following the style of the existing DbSets.

## 5. IEntityTypeConfiguration

**Path:** `/src/Core/RaTemplate.Persistence/Configurations/EntityNameConfiguration.cs`

- Class is `internal sealed`, named `EntityName` + `Configuration` (e.g. `TodoConfiguration`), and implements `IEntityTypeConfiguration<EntityName>`.
- Use the **Fluent API only**. No data annotations on the entity.
- Use the constants from `EntityNameConstraints` for lengths, never literal numbers.
- Configure keys, required/optional, max lengths, indexes and relationships as appropriate.

### Value converters and extensions

For these property types use the helpers from the package `RA.Utilities.Data.EntityFramework`.

| Property type   | Use                                                                 | Namespace                                          |
|-----------------|---------------------------------------------------------------------|----------------------------------------------------|
| `Currency`      | `HasConversion<CurrencyConverter>()`                                | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `Email`         | `HasConversion<EmailConverter>()`                                   | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `SSN`           | `HasConversion<SsnConverter>()`                                     | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `IbanConverter` | `HasConversion<IbanConverter>()`                                    | `RA.Utilities.Data.EntityFramework.Conventions`    |
| `Money`         | `HasMoney()` extension method (do **not** use a `ValueConverter`)   | `RA.Utilities.Data.EntityFramework.Extensions`     |

Add the matching `using` for each helper you use.

```csharp
using RA.Utilities.Data.EntityFramework.Conventions;
using RA.Utilities.Data.EntityFramework.Extensions;

internal sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
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

        builder.Property(t => t.Price)
            .HasMoney();
    }
}
```

For `Money`, check the actual `HasMoney` signature in the installed package (go to definition) and call it the way it is declared.
If it takes extra arguments or is declared on a different builder type, follow that and keep the rule: `HasMoney`, never `MoneyConverter`.

## Final checklist

Before reporting done, confirm:

- [ ] Entity is `sealed`, in `/src/Core/RaTemplate.Domain/Entities`, with the correct base class
- [ ] Every entity property has multi-line XML docs
- [ ] `EntityNameConstraints` exists (if needed) and no magic numbers remain in the configuration
- [ ] `DbSet` (plural) added to the abstraction and to the implementing DbContext
- [ ] `EntityNameConfiguration` is `internal sealed`, Fluent API only, with the right converters
- [ ] Solution builds (`dotnet build`) if the environment allows it

Do not create an EF migration unless the user asks for one.