using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.Repository.IRepository;

public interface IProductColorRepository : IBaseRepository<ProductColor>
{
    Task<int> AddAsync(List<StockItemDto> stocks, Guid ProductID);
}
