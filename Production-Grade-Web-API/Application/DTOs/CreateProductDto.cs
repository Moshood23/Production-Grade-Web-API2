using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Application.DTO
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryId { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Active;
    }
}