
using Microsoft.EntityFrameworkCore;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Infrastructure.Data;

namespace Production.Grade.WebApi.Infrastructure.Services;
public class OrderNumberGenerationService : IOrderNumberGenerationService
{
    private readonly ApplicationDbContext _context;

    public OrderNumberGenerationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var ordersCreatedToday = await _context.Orders
            .Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date)
            .CountAsync();

        var sequenceNumber = (ordersCreatedToday + 1).ToString("D5");
        return $"ORD-{today}-{sequenceNumber}";
    }
}

