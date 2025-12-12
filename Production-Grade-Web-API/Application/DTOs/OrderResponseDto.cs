using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Application.DTO
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; } = new();
        public string? Notes { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}