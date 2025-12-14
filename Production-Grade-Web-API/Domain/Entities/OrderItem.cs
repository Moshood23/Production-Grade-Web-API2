namespace Production.Grade.WebApi.Domain.Entities;
public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtTime { get; set; }
    public int LineTotal => (int)(Quantity * PriceAtTime);
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}