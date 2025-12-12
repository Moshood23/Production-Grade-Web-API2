namespace Production.Grade.WebApi.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtAddTime { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public int LineTotal => (int)(Quantity * PriceAtAddTime);
    public Cart? Cart { get; set; }
    public Product? Product { get; set; }
}