using ECommerce.BLL.IRepository;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> AddToChart(int ID);
    }
}
