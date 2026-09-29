using Microsoft.EntityFrameworkCore;
using RaTemplate.Application.Abstractions.Data;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the SQLite database context.
/// </summary>
public sealed class RaTemplateSqliteDbContext(DbContextOptions<RaTemplateSqliteDbContext> options)
    : DbContext(options), IRaTemplateSqliteDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplateSqliteDbContext).Assembly);
    }
}
