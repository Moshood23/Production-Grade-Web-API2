namespace Production.Grade.WebApi.Domain.Interfaces;

/// <summary>
/// Generic repository interface for CRUD operations.
/// All repositories implement this contract for consistent data access.
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get entity by its primary key (ID)
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns>Entity or null if not found</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all entities (applies global query filters automatically)
    /// </summary>
    /// <returns>Collection of entities</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get all entities asynchronously with paging support
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Paginated collection of entities</returns>
    Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Find entities matching a predicate
    /// </summary>
    /// <param name="predicate">Filter expression</param>
    /// <returns>Collection of matching entities</returns>
    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);

    /// <summary>
    /// Add a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns>The added entity</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Add multiple entities in batch
    /// </summary>
    /// <param name="entities">Entities to add</param>
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync();
    Task<IEnumerable<T>> FromSqlAsync(string sql);
}