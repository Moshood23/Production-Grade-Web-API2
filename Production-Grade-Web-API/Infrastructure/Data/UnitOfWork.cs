namespace Production.Grade.WebApi.Infrastructure.Data;

using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;
using Production.Grade.WebApi.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _transaction;

    private IRepository<Category>? _categoryRepository;
    private IRepository<Product>? _productRepository;
    private IRepository<Order>? _orderRepository;
    private IRepository<OrderItem>? _orderItemRepository;
    private IRepository<Picture>? _pictureRepository;
    private IRepository<Cart>? _cartRepository;
    private IRepository<CartItem>? _cartItemRepository;
    private IRepository<ApplicationUser>? _userRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<Category> Categories =>
        _categoryRepository ??= new BaseRepository<Category>(_context);

    public IRepository<Product> Products =>
        _productRepository ??= new BaseRepository<Product>(_context);

    public IRepository<Order> Orders =>
        _orderRepository ??= new BaseRepository<Order>(_context);

    public IRepository<OrderItem> OrderItems =>
        _orderItemRepository ??= new BaseRepository<OrderItem>(_context);

    public IRepository<Picture> Pictures =>
        _pictureRepository ??= new BaseRepository<Picture>(_context);

    public IRepository<Cart> Carts =>
        _cartRepository ??= new BaseRepository<Cart>(_context);

    public IRepository<CartItem> CartItems =>
        _cartItemRepository ??= new BaseRepository<CartItem>(_context);

    public IRepository<ApplicationUser> Users =>
        _userRepository ??= new BaseRepository<ApplicationUser>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable)
    {
        _transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
        return _transaction;
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

    Task<Domain.Interfaces.IDbContextTransaction> IUnitOfWork.BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        throw new NotImplementedException();
    }
}