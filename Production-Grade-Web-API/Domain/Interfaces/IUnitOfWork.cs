namespace Production.Grade.WebApi.Domain.Interfaces;

using Production.Grade.WebApi.Domain.Entities;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
   
    IRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderItem> OrderItems { get; }
    IRepository<Picture> Pictures { get; } 
    IRepository<Cart> Carts { get; }
    IRepository<CartItem> CartItems { get; }
    IRepository<ApplicationUser> Users { get; }
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable);
    Task CommitAsync();
    Task RollbackAsync();
}
public interface IDbContextTransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();

    Task RollbackAsync();

    Guid TransactionId { get; }
}