namespace Production_Grade_Web_API.Application.DTO
{
    public class CartItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtAddTime { get; set; }
        public decimal LineTotal { get; set; }
        public DateTime AddedAt { get; set; }
    }
}