using System;
using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
            : base(context) => _context = context;

        public async Task<Product> AddToChart(int ID)
        {
            var Product = await FindAsync(ID);
            return Product;
        }

        public async Task<Product> GetProductItemAsync(Guid iD)
        {
            var product = await _context
                .Products.Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .AsNoTracking()
                .FirstAsync(x => x.Id == iD);

            return product;
        }
    }
}
