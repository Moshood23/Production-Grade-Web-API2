namespace Production.Grade.WebApi.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Production.Grade.WebApi.Domain.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplyGlobalQueryFilters(modelBuilder);
        ApplyEntityConfigurations(modelBuilder);
    }

    private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType);
                var propertyAccess = System.Linq.Expressions.Expression.PropertyOrField(parameter, nameof(BaseEntity.IsDeleted));
                var isDeletedFalse = System.Linq.Expressions.Expression.Equal(propertyAccess, System.Linq.Expressions.Expression.Constant(false));
                var lambda = System.Linq.Expressions.Expression.Lambda(isDeletedFalse, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    private void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        baseEntity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            if (entry.Entity is Cart cart)
            {
                if (entry.State == EntityState.Modified)
                {
                    cart.LastModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}