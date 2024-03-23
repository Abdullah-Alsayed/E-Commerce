// Ignore Spelling: BLL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository
{
    public class ProductSizeRepository : BaseRepository<ProductSize>, IProductSizeRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductSizeRepository(ApplicationDbContext context)
            : base(context) => _context = context;

        public async Task<int> AddAsync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rowsAffected = 0;
            var existingProductSizes = await _context
                .ProductSizes.Where(x => x.ProductID == ProductID)
                .ToListAsync();

            foreach (var stockItem in stocks)
            {
                var existingProductSize = existingProductSizes.FirstOrDefault(x =>
                    x.SizeID == stockItem.ID
                );
                if (existingProductSize != null)
                    existingProductSize.Quantity += stockItem.Quantity;
                else
                    await _context.ProductSizes.AddAsync(
                        new ProductSize
                        {
                            ProductID = ProductID,
                            SizeID = stockItem.ID,
                            Quantity = stockItem.Quantity
                        }
                    );
                rowsAffected++;
            }
            await _context.SaveChangesAsync();
            return rowsAffected;
        }
    }
}
