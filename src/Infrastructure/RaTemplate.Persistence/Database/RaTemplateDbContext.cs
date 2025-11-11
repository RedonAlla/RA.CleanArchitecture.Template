using Microsoft.EntityFrameworkCore;
using RA.Utilities.Data.Abstractions;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the database context for Todo items.
/// </summary>
public sealed class RaTemplateDbContext(DbContextOptions<RaTemplateDbContext> options) : DbContext(options), IDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplateDbContext).Assembly);
    }
}
