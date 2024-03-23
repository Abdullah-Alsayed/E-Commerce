using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.BLL.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> AddToChart(int ID);
        Task<Product> GetProductItemAsync(Guid iD);
    }
}
