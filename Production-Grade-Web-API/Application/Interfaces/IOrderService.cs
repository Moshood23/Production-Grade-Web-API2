using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto dto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid id, string userId);
        Task<IEnumerable<OrderListDto>> GetUserOrdersAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<OrderResponseDto> CancelOrderAsync(Guid id, string userId);
        Task<OrderResponseDto> CheckoutCartAsync(string userId, CreateOrderDto? dto = null);
    }

}
