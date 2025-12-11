namespace Production.Grade.WebApi.Application.Services;

using Microsoft.EntityFrameworkCore;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Domain.Enums;
using Production.Grade.WebApi.Domain.Interfaces;
using Production.Grade.WebApi.Infrastructure.Data;
using Production_Grade_Web_API.Application.DTO;
using Production_Grade_Web_API.Application.Interfaces;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IOrderNumberGenerationService _orderNumberService;

    public OrderService(IUnitOfWork unitOfWork, ApplicationDbContext context, IMapper mapper, IOrderNumberGenerationService orderNumberService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _mapper = mapper;
        _orderNumberService = orderNumberService;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        using var transaction = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            var order = new Order
            {
                OrderNumber = await _orderNumberService.GenerateOrderNumberAsync(),
                CreatedBy = userId,
                UserId = user.Id,
                Status = OrderStatus.Pending,
                Notes = dto.Notes
            };

            decimal totalPrice = 0;

            foreach (var item in dto.OrderItems)
            {
                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId && p.UserId == user.Id);

                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");

                if (product.Quantity < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.Quantity}, Requested: {item.Quantity}");

                if (product.Status != ProductStatus.Active)
                    throw new InvalidOperationException($"Product {product.Name} is not available for purchase");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    PriceAtTime = product.Price,
                    CreatedBy = userId
                };

                order.OrderItems.Add(orderItem);
                totalPrice += orderItem.LineTotal;

                product.Quantity -= item.Quantity;
                if (product.Quantity == 0)
                    product.Status = ProductStatus.OutOfStock;

                _context.Products.Update(product);
            }

            order.TotalPrice = totalPrice;
            order.Status = OrderStatus.Confirmed;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderResponseDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(Guid id, string userId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null || order.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Order with ID {id} not found");

        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<IEnumerable<OrderListDto>> GetUserOrdersAsync(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var orders = await _unitOfWork.Orders.GetAllAsync(pageNumber, pageSize);
        var userOrders = orders
            .Where(o => o.UserId == Guid.Parse(userId))
            .ToList();

        return _mapper.Map<IEnumerable<OrderListDto>>(userOrders);
    }

    public async Task<OrderResponseDto> CancelOrderAsync(Guid id, string userId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null || order.UserId != Guid.Parse(userId))
            throw new KeyNotFoundException($"Order with ID {id} not found");

        if (order.Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        if (order.Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be cancelled");

        using var transaction = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    if (product.Status == ProductStatus.OutOfStock)
                        product.Status = ProductStatus.Active;

                    _context.Products.Update(product);
                }
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedBy = userId;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderResponseDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderResponseDto> CheckoutCartAsync(string userId, CreateOrderDto? dto = null)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        var cart = user.Cart;
        if (cart == null || cart.CartItems.Count == 0)
            throw new InvalidOperationException("Cart is empty");

        var orderDto = new CreateOrderDto
        {
            OrderItems = cart.CartItems.Select(ci => new CreateOrderItemDto
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity
            }).ToList()
        };

        var order = await CreateOrderAsync(userId, orderDto);

        await _unitOfWork.CartItems.DeleteRangeAsync(cart.CartItems);
        cart.LastModifiedAt = DateTime.UtcNow;
        await _unitOfWork.Carts.UpdateAsync(cart);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }
}