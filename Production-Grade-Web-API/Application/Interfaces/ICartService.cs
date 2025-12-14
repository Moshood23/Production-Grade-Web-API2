using AutoMapper;
using Production.Grade.WebApi.Application.DTO;

namespace Production.Grade.WebApi.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(string userId);
        Task<CartItemResponseDto> AddItemAsync(string userId, CreateCartItemDto dto);
        Task<CartItemResponseDto> UpdateItemAsync(string userId, Guid itemId, UpdateCartItemDto dto);
        Task RemoveItemAsync(string userId, Guid itemId);
        Task ClearCartAsync(string userId);
        Task<CartResponseDto> GetCartDetailsAsync(string userId);
    }

}
