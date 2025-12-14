using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Application.DTO
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
