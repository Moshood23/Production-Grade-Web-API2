namespace Production.Grade.WebApi.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);

    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);

    Task<T> AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync();
    Task<IEnumerable<T>> FromSqlAsync(string sql);
}