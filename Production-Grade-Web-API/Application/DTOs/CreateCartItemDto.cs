namespace Production.Grade.WebApi.Application.DTO
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
