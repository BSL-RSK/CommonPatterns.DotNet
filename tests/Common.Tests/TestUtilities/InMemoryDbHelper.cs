using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Tests.TestUtilities;

/// <summary>
/// Utility class to register and manage an in-memory Entity Framework Core database for testing.
/// </summary>
public static class InMemoryDbHelper
{
    #region Registration

    /// <summary>
    /// Adds an in-memory database context to the service collection for testing.
    /// </summary>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/> to register.</typeparam>
    /// <param name="services">The service collection to add the context to.</param>
    /// <param name="databaseName">Optional database name (useful to isolate tests).</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInMemoryDbContext<TContext>(this IServiceCollection services, string? databaseName = null)
        where TContext : DbContext
    {
        return services.AddDbContext<TContext>(options =>
            options.UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString()));
    }

    #endregion

    #region Reset

    /// <summary>
    /// Clears all data in the given in-memory database context.
    /// </summary>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/>.</typeparam>
    /// <param name="context">The instance of the database context.</param>
    public static void ResetDatabase<TContext>(TContext context)
        where TContext : DbContext
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    #endregion
}