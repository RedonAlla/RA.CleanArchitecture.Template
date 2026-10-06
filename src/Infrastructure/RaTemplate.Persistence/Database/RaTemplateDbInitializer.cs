#pragma warning disable S1144
#pragma warning disable S1172

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RaTemplate.Persistence.Database;

/// <summary>
/// Provides extension methods for database initialization.
/// </summary>
public static class RaTemplateDbInitializer
{
    /// <summary>
    /// Initializes and seeds the application databases.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider" /> used to resolve the database initializers.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        RaTemplateSqlServerDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<RaTemplateSqlServerDbContext>();

        ILogger<RaTemplateSqlServerDbContext> logger =
            scope.ServiceProvider.GetRequiredService<ILogger<RaTemplateSqlServerDbContext>>();

        //#if (UseEfSqlServer)
        await InitializeAsync(dbContext, logger);
        //#endif
    }

    /// <summary>
    /// Creates the database if it does not exist.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    private static async Task InitializeAsync(RaTemplateSqlServerDbContext context, ILogger logger)
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            string sql = context.Database.GenerateCreateScript();
            logger.LogInformation("======================= Data Base {DbContext} script =======================", typeof(RaTemplateSqlServerDbContext).Name);
            logger.LogInformation("{SqlScript}", sql);
            logger.LogInformation("============================================================================");
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
    private static async Task SeedAsync(RaTemplateSqlServerDbContext context, ILogger logger)
    {
        try
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

            //logger.LogInformation("Seeded {Count} sample todos", todos.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
