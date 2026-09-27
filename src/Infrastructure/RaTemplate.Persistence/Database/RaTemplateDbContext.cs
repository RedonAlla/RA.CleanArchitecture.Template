using Microsoft.EntityFrameworkCore;
using RaTemplate.Application.Abstractions.Data;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the database context for Todo items.
/// </summary>
public sealed class RaTemplateDbContext(DbContextOptions<RaTemplateDbContext> options) : DbContext(options), IRaTemplateDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Default);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplateDbContext).Assembly);
    }

    ValueTask<int> IRaTemplateDbContext.SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
