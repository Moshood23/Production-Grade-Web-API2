using Production.Grade.WebApi.Domain.Enums;

namespace Production.Grade.WebApi.Application.DTO
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
    }
}
