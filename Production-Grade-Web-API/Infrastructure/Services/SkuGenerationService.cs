
namespace Production.Grade.WebApi.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Production.Grade.WebApi.Application.Interfaces;
using Production.Grade.WebApi.Infrastructure.Data;
using Production_Grade_Web_API.Application.Interfaces;

public class SkuGenerationService : ISkuGenerationService
{
    private readonly ApplicationDbContext _context;

    public SkuGenerationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateSkuAsync()
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var productsCreatedToday = await _context.Products
            .Where(p => p.CreatedAt.Date == DateTime.UtcNow.Date)
            .CountAsync();

        var sequenceNumber = (productsCreatedToday + 1).ToString("D5");
        return $"SKU-{today}-{sequenceNumber}";
    }
}
