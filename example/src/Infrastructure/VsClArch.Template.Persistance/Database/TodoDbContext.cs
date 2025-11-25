using System;
using Microsoft.EntityFrameworkCore;
using RA.Utilities.Data.Abstractions;
using VsClArch.Template.Domain.Entities;

namespace VsClArch.Template.Persistence.Database;

/// <summary>
/// Represents the database context for Todo items.
/// </summary>
public sealed class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options), IDbContext
{
    /// <inheritdoc/>
    public DbSet<Todo> Todos { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
    }
}
