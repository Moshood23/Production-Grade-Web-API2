namespace Production.Grade.WebApi.Application.DTO
{
    public class OrderItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int PriceAtTime { get; set; }
        public int LineTotal { get; set; }
    }
}