namespace Production.Grade.WebApi.Application.DTO
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
        public string? Notes { get; set; }

    }
}