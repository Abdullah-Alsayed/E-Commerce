using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IStockRepository : IBaseRepository<Stock>
    {
        Task<int> AddStockAsync(Stock stock, CreateStockRequest request);
        Task<int> ReturnItemAsync(ReturnStockRequest request);
    }
}
