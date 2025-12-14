using AutoMapper;
using Production.Grade.WebApi.Application.DTOs.Category;
using Production.Grade.WebApi.Application.DTOs;

namespace Production.Grade.WebApi.Application.Interfaces
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
