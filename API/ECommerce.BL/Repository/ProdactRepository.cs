using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.BLL.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(Applicationdbcontext context)
            : base(context) { }

        public async Task<Product> AddToChart(int ID)
        {
            var Product = await FindAsync(ID);
            return Product;
        }
    }
}
