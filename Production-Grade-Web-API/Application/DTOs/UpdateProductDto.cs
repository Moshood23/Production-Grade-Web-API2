using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Application.DTO
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public Guid? CategoryId { get; set; }
        public ProductStatus? Status { get; set; }
    }
}
