namespace Production_Grade_Web_API.Application.DTO
{
    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
