using Production.Grade.WebApi.Domain.Enums;

namespace Production_Grade_Web_API.Application.DTO
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
    }
}
