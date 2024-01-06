using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using ECommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(Applicationdbcontext context, IHostingEnvironment hosting)
            : base(context, hosting) { }

        public async Task<Product> AddToChart(int ID)
        {
            var Product = await FindAsync(ID);
            return Product;
        }
    }
}
