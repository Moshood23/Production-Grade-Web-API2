namespace Production_Grade_Web_API.Application.DTO
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
