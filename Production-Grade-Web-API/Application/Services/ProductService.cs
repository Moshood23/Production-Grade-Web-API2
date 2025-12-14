
using AutoMapper;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;
using Production.Grade.WebApi.Application.DTO;

namespace Production.Grade.WebApi.Application.Services;
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    private readonly ISkuGenerationService _skuService;

    public ProductService(IUnitOfWork unitOfWork, AutoMapper.IMapper mapper, ISkuGenerationService skuService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _skuService = skuService;
    }

    public async Task<ProductResponseDto> CreateAsync(string userId, CreateProductDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null || category.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found");

        var product = _mapper.Map<Product>(dto);
        product.CreatedBy = userId;
        product.UserId = Guid.Parse(userId);
        product.SKU = await _skuService.GenerateSkuAsync();

        if (product.Quantity == 0)
            product.Status = Domain.Enums.ProductStatus.OutOfStock;

        var result = await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(result);
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id, string userId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null || product.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Product with ID {id} not found");

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> GetBySkuAsync(string sku, string userId)
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        var product = products.FirstOrDefault(p => p.SKU == sku && p.UserId == Guid.Parse(userId));

        if (product == null)
            throw new KeyNotFoundException($"Product with SKU {sku} not found");

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<IEnumerable<ProductListDto>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var products = await _unitOfWork.Products.GetAllAsync(pageNumber, pageSize);
        var userProducts = products
            .Where(p => p.UserId == Guid.Parse(userId))
            .ToList();

        return _mapper.Map<IEnumerable<ProductListDto>>(userProducts);
    }

    public async Task<IEnumerable<ProductListDto>> GetByCategoryAsync(Guid categoryId, string userId, int pageNumber = 1, int pageSize = 10)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (category == null || category.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Category with ID {categoryId} not found");

        var products = await _unitOfWork.Products.GetAllAsync();
        var categoryProducts = products
            .Where(p => p.CategoryId == categoryId && p.UserId == Guid.Parse(userId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<IEnumerable<ProductListDto>>(categoryProducts);
    }

    public async Task<ProductResponseDto> UpdateAsync(Guid id, string userId, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null || product.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Product with ID {id} not found");

        if (dto.CategoryId.HasValue && dto.CategoryId.Value != product.CategoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId.Value);
            if (category == null || category.UserId != Guid.Parse(userId))
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found");
            product.CategoryId = dto.CategoryId.Value;
        }

        if (!string.IsNullOrEmpty(dto.Name))
            product.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Description))
            product.Description = dto.Description;

        if (dto.Price.HasValue)
            product.Price = dto.Price.Value;

        if (dto.Quantity.HasValue)
            product.Quantity = dto.Quantity.Value;

        if (dto.Status.HasValue)
            product.Status = dto.Status.Value;

        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        if (product.Quantity == 0 && product.Status != Domain.Enums.ProductStatus.OutOfStock)
            product.Status = Domain.Enums.ProductStatus.OutOfStock;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task DeleteAsync(Guid id, string userId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null || product.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Product with ID {id} not found");

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> CheckStockAsync(Guid productId, int quantity)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        return product != null && product.Quantity >= quantity && product.Status == Domain.Enums.ProductStatus.Active;
    }
}