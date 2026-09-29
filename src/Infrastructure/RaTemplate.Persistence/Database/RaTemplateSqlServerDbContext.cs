using Microsoft.EntityFrameworkCore;
using RaTemplate.Application.Abstractions.Data;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the SQL Server database context.
/// </summary>
public sealed class RaTemplateSqlServerDbContext(DbContextOptions<RaTemplateSqlServerDbContext> options)
    : DbContext(options), IRaTemplateSqlServerDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Default);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplateSqlServerDbContext).Assembly);
    }
}
