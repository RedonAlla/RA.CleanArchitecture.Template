using Microsoft.EntityFrameworkCore;
using RaTemplate.Application.Abstractions.Data;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Represents the Oracle database context.
/// </summary>
public sealed class RaTemplateOracleDbContext(DbContextOptions<RaTemplateOracleDbContext> options)
    : DbContext(options), IRaTemplateOracleDbContext
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaTemplateOracleDbContext).Assembly);
    }
}
