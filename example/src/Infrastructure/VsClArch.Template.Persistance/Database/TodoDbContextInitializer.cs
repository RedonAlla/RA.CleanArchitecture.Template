using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VsClArch.Template.Domain.Entities;


namespace VsClArch.Template.Persistence.Database;

/// <summary>
/// Provides extension methods for <see cref="TodoDbContextInitializer"/>.
/// </summary>
public static class InitializerExtensions
{
    /// <summary>
    /// Initializes and seeds the database asynchronously.
    /// </summary>
    /// <param name="services">The <see cref="IServiceScope"/> to resolve services from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task InitializeDatabaseAsync(this IServiceScope services)
    {
        TodoDbContextInitializer initializer =
            services.ServiceProvider.GetRequiredService<TodoDbContextInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}

internal sealed class TodoDbContextInitializer
{
    private readonly ILogger<TodoDbContextInitializer> _logger;
    private readonly TodoDbContext _context;

    public TodoDbContextInitializer(ILogger<TodoDbContextInitializer> logger, TodoDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
#if DEBUG
            // In development, we can delete and recreate the database.
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies for more details.
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
#endif
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw new InvalidOperationException("Database initialization failed.", ex);
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw new InvalidOperationException("Database seeding failed.", ex);
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Todos.Any())
        {
            _context.Todos.AddRange(
                new Todo
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Description = "To DO 1",
                    DueDate = DateTime.UtcNow.AddDays(20),
                    Labels = ["FrontEnd", "Backend"],
                    IsCompleted = false,
                    CreatedAt = DateTime.Today
                },
                new Todo
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Description = "To DO 2",
                    DueDate = DateTime.UtcNow.AddDays(20),
                    Labels = ["FrontEnd"],
                    IsCompleted = false,
                    CreatedAt = DateTime.Today
                });

            await _context.SaveChangesAsync();
        }
    }
}
