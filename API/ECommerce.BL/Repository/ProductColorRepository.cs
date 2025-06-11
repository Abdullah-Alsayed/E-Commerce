using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository;

public class ProductColorRepository : BaseRepository<ProductColor>, IProductColorRepository
{
    private readonly ApplicationDbContext _context;

    public ProductColorRepository(ApplicationDbContext context)
        : base(context) => _context = context;

    public async Task<int> AddAsync(List<StockItemDto> stocks, Guid ProductID)
    {
        var rowsAffected = 0;
        var existingProductColors = await _context
            .ProductColors.Where(x => x.ProductID == ProductID)
            .ToListAsync();

        foreach (var stockItem in stocks)
        {
            var existingProductColor = existingProductColors.FirstOrDefault(x =>
                x.ColorID == stockItem.ID
            );
            if (existingProductColor != null)
                existingProductColor.Quantity += stockItem.Quantity;
            else
                await _context.ProductColors.AddAsync(
                    new ProductColor
                    {
                        ProductID = ProductID,
                        ColorID = stockItem.ID,
                        Quantity = stockItem.Quantity
                    }
                );
            rowsAffected++;
        }
        await _context.SaveChangesAsync();
        return rowsAffected;
    }
}
