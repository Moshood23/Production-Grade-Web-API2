namespace Production.Grade.WebApi.Infrastructure.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            pageNumber = 1;
            pageSize = 10;
        }

        return await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate).ToList());
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }

        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<IEnumerable<T>> FromSqlAsync(string sql)
    {
        return await _dbSet.FromSqlRaw(sql).ToListAsync();
    }
}