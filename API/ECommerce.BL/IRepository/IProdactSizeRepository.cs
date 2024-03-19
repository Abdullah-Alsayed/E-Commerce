using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IProdactSizeRepository : IBaseRepository<ProductSize>
    {
        Task<int> AddaAync(List<StockItemDto> stocks, Guid ProductID);
    }
}
