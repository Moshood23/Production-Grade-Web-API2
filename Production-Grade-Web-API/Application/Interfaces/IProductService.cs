using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(string userId, CreateProductDto dto);
        Task<ProductResponseDto> GetByIdAsync(Guid id, string userId);
        Task<ProductResponseDto> GetBySkuAsync(string sku, string userId);
        Task<IEnumerable<ProductListDto>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<ProductListDto>> GetByCategoryAsync(Guid categoryId, string userId, int pageNumber = 1, int pageSize = 10);
        Task<ProductResponseDto> UpdateAsync(Guid id, string userId, UpdateProductDto dto);
        Task DeleteAsync(Guid id, string userId);
        Task<bool> CheckStockAsync(Guid productId, int quantity);
    }

}
