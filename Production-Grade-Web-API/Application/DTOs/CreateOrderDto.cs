namespace Production_Grade_Web_API.Application.DTO
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
        public string? Notes { get; set; }

    }
}