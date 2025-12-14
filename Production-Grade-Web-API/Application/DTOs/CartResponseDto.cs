namespace Production.Grade.WebApi.Application.DTO
{
    public class CartResponseDto
    {
        public Guid Id { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public int TotalPrice { get; set; }
        public int TotalItemCount { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
