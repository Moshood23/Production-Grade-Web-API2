namespace Production.Grade.WebApi.Domain.Entities;

/// <summary>
/// Represents a single item/product in a shopping cart.
/// Users can have multiple CartItems in their Cart.
/// CartItems are converted to OrderItems when order is placed.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Foreign key to the Cart this item belongs to
    /// </summary>
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtAddTime { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public decimal LineTotal => Quantity * PriceAtAddTime;
    public Cart? Cart { get; set; }
    public Product? Product { get; set; }
}