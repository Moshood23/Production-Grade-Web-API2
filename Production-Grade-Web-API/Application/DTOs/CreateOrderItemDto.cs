namespace Production.Grade.WebApi.Application.DTO
{
    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
