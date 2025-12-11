using Production.Grade.WebApi.Domain.Enums;

namespace Production_Grade_Web_API.Application.DTO
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
