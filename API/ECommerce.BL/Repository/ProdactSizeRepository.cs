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
    public class ProdactSizeRepository : BaseRepository<ProductSize>, IProdactSizeRepository
    {
        private readonly Applicationdbcontext _context;

        public ProdactSizeRepository(Applicationdbcontext context)
            : base(context) => _context = context;

        public async Task<int> AddaAync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rows = 0;
            var Ids = stocks.Select(x => x.ID);
            var productsSize = await _context
                .ProductSizes.Where(x => Ids.Contains(x.SizeId))
                .ToListAsync();
            foreach (var prodct in productsSize)
            {
                prodct.Quantity += stocks.FirstOrDefault(x => x.ID == prodct.ID).Quantity;
                rows++;
            }
            return rows;
        }
    }
}
