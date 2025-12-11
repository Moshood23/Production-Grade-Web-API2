using Production.Grade.WebApi.Application.Mappings;
using Production.Grade.WebApi.Application.Services;

namespace Production_Grade_Web_API.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateAsync(string userId, CreateCategoryDto dto);
        Task<CategoryResponseDto> GetByIdAsync(Guid id, string userId);
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync(string userId);
        Task<CategoryResponseDto> UpdateAsync(Guid id, string userId, UpdateCategoryDto dto);
        Task DeleteAsync(Guid id, string userId);
    }

}
