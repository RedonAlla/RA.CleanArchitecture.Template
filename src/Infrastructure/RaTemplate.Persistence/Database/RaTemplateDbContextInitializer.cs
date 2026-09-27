using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Provides extension methods for database initialization.
/// </summary>
public static class RaTemplateDbContextInitializerExtensions
{
    /// <summary>
    /// Initializes and seeds the application database.
    /// </summary>
    /// <param name="scope">The <see cref="IServiceScope" /> used to resolve the database initializer.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task InitializeDatabaseAsync(this IServiceScope scope)
    {
        RaTemplateDbContextInitializer initializer =
            scope.ServiceProvider.GetRequiredService<RaTemplateDbContextInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}

/// <summary>
/// Creates and seeds the application database.
/// </summary>
public sealed class RaTemplateDbContextInitializer(
    ILogger<RaTemplateDbContextInitializer> logger,
    RaTemplateDbContext context)
{
    /// <summary>
    /// Creates the database if it does not exist.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with default data.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    /// <summary>
    /// Attempts to seed the database with default data, if necessary.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        // if (!context.TodoLists.Any())
        // {
        //     context.TodoLists.Add(new TodoList
        //     {
        //     });

        //     await context.SaveChangesAsync();
        // }

        await Task.CompletedTask;
    }
}
