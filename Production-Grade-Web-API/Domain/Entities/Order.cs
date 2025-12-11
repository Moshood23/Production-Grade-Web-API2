using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Domain.Entities;
public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ApplicationUser? User { get; set; }
}