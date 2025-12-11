using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Domain.Entities;
public class Product : BaseEntity
{
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public byte[]? RowVersion { get; set; }
    public Category? Category { get; set; }
    public ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ApplicationUser? User { get; set; }
}