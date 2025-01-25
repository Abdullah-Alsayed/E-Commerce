// Ignore Spelling: BLL

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IProductSizeRepository : IBaseRepository<ProductSize>
    {
        Task<int> AddAsync(List<StockItemDto> stocks, Guid ProductID);
    }
}
