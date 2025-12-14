namespace Production.Grade.WebApi.Application.Services;
using AutoMapper;
using Production.Grade.WebApi.Application.DTOs.Category;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;
using Production.Grade.WebApi.Application.DTOs;
using IMapper = AutoMapper.IMapper;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryResponseDto> CreateAsync(string userId, CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        category.CreatedBy = userId;
        category.UserId = Guid.Parse(userId);

        var result = await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponseDto>(result);
    }

    public async Task<CategoryResponseDto> GetByIdAsync(Guid id, string userId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null || category.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Category with ID {id} not found");

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(string userId)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var userCategories = categories
            .Where(c => c.UserId == Guid.Parse(userId))
            .ToList();

        return _mapper.Map<IEnumerable<CategoryResponseDto>>(userCategories);
    }

    public async Task<CategoryResponseDto> UpdateAsync(Guid id, string userId, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null || category.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Category with ID {id} not found");

        if (!string.IsNullOrEmpty(dto.Name))
            category.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Description))
            category.Description = dto.Description;

        category.UpdatedBy = userId;
        category.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task DeleteAsync(Guid id, string userId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null || category.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Category with ID {id} not found");

        await _unitOfWork.Categories.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
}
