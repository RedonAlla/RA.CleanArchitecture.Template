namespace RaTemplate.Application.Abstractions.Data;

/// <summary>
/// Represents the Entity Framework database context for the RaTemplate system.
/// </summary>
public interface IRaTemplateDbContext
{
    /// <summary>
    ///     Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the asynchronous save operation.
    ///     The task result contains the number of state entries written to the database.
    /// </returns>
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken);
}
