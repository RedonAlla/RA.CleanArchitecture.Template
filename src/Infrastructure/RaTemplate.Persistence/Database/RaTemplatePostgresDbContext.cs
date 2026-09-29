using Microsoft.EntityFrameworkCore;
using RaTemplate.Application.Abstractions.Data;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the PostgreSQL database context.
/// </summary>
public sealed class RaTemplatePostgresDbContext(DbContextOptions<RaTemplatePostgresDbContext> options)
    : DbContext(options), IRaTemplatePostgresDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplatePostgresDbContext).Assembly);
    }
}
