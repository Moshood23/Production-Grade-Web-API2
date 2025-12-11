namespace Production.Grade.WebApi.Application.Services;

using AutoMapper;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Interfaces;
using Production_Grade_Web_API.Application.DTO;
using Production_Grade_Web_API.Application.Interfaces;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CartResponseDto> GetCartAsync(string userId)
    {
        var user = await GetUserOrThrowAsync(userId);
        var cart = user.Cart;

        if (cart == null)
            throw new KeyNotFoundException("Cart not found for user");

        return _mapper.Map<CartResponseDto>(cart);
    }

    public async Task<CartItemResponseDto> AddItemAsync(string userId, CreateCartItemDto dto)
    {
        var user = await GetUserOrThrowAsync(userId);
        var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);

        if (product == null || product.UserId != user.Id)
            throw new KeyNotFoundException($"Product with ID {dto.ProductId} not found");

        if (product.Quantity < dto.Quantity)
            throw new InvalidOperationException($"Insufficient stock. Available: {product.Quantity}");

        var cart = user.Cart;
        if (cart == null)
        {
            cart = new Cart { UserId = user.Id, CreatedBy = userId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
            existingItem.PriceAtAddTime = product.Price;
            await _unitOfWork.CartItems.UpdateAsync(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                PriceAtAddTime = product.Price,
                CreatedBy = userId
            };
            await _unitOfWork.CartItems.AddAsync(cartItem);
        }

        cart.LastModifiedAt = DateTime.UtcNow;
        await _unitOfWork.Carts.UpdateAsync(cart);
        await _unitOfWork.SaveChangesAsync();

        var cartItem2 = cart.CartItems.First(ci => ci.ProductId == dto.ProductId);
        return _mapper.Map<CartItemResponseDto>(cartItem2);
    }

    public async Task<CartItemResponseDto> UpdateItemAsync(string userId, Guid itemId, UpdateCartItemDto dto)
    {
        var user = await GetUserOrThrowAsync(userId);
        var cartItem = await _unitOfWork.CartItems.GetByIdAsync(itemId);

        if (cartItem == null || cartItem.Cart?.UserId != user.Id)
            throw new KeyNotFoundException($"Cart item with ID {itemId} not found");

        var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
        if (product == null || product.Quantity < dto.Quantity)
            throw new InvalidOperationException($"Insufficient stock. Available: {product.Quantity}");

        cartItem.Quantity = dto.Quantity;
        cartItem.UpdatedBy = userId;
        cartItem.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CartItems.UpdateAsync(cartItem);

        var cart = cartItem.Cart;
        if (cart != null)
        {
            cart.LastModifiedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);
        }

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CartItemResponseDto>(cartItem);
    }

    public async Task RemoveItemAsync(string userId, Guid itemId)
    {
        var user = await GetUserOrThrowAsync(userId);
        var cartItem = await _unitOfWork.CartItems.GetByIdAsync(itemId);

        if (cartItem == null || cartItem.Cart?.UserId != user.Id)
            throw new KeyNotFoundException($"Cart item with ID {itemId} not found");

        await _unitOfWork.CartItems.DeleteAsync(cartItem);

        var cart = cartItem.Cart;
        if (cart != null)
        {
            cart.LastModifiedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ClearCartAsync(string userId)
    {
        var user = await GetUserOrThrowAsync(userId);
        var cart = user.Cart;

        if (cart != null)
        {
            await _unitOfWork.CartItems.DeleteRangeAsync(cart.CartItems);
            cart.LastModifiedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CartResponseDto> GetCartDetailsAsync(string userId)
    {
        var user = await GetUserOrThrowAsync(userId);
        var cart = user.Cart;

        if (cart == null)
            throw new KeyNotFoundException("Cart not found for user");

        return _mapper.Map<CartResponseDto>(cart);
    }

    private async Task<ApplicationUser> GetUserOrThrowAsync(string userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");
        return user;
    }
}
