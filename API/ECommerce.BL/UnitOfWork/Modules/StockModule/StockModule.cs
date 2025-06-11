using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.StockModule
{
    public class StockModule : IStockModule
    {
        private readonly ApplicationDbContext _context;

        public StockModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductSizeRepository ProductSize =>
            _productSize ??= new ProductSizeRepository(_context);
        private IProductSizeRepository _productSize;

        public IProductColorRepository ProductColor =>
            _productColor ??= new ProductColorRepository(_context);
        private IProductColorRepository _productColor;

        public IStockRepository Stock => _stock ??= new StockRepository(_context);
        private IStockRepository _stock;

        public IBaseRepository<Vendor> Vendor => _vendor ??= new BaseRepository<Vendor>(_context);
        private IBaseRepository<Vendor> _vendor;
    }
}
