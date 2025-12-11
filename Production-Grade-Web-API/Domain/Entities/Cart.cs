namespace Production.Grade.WebApi.Domain.Entities;
public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    public decimal CalculatedTotalPrice => CartItems.Sum(ci => ci.Quantity * ci.PriceAtAddTime);
    public int TotalItemCount => CartItems.Sum(ci => ci.Quantity);
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ApplicationUser? User { get; set; }
}