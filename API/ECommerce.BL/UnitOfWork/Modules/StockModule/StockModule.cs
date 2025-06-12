using System;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.StockModule
{
    public class StockModule : IStockModule
    {
        private readonly ApplicationDbContext _context;

        // Replace private fields with Lazy<T>
        private readonly Lazy<IProductSizeRepository> _productSize;
        private readonly Lazy<IProductColorRepository> _productColor;
        private readonly Lazy<IStockRepository> _stock;
        private readonly Lazy<IBaseRepository<Vendor>> _vendor;

        public StockModule(ApplicationDbContext context)
        {
            _context = context;

            // Initialize Lazy<T> instances
            _productSize = new Lazy<IProductSizeRepository>(
                () => new ProductSizeRepository(_context)
            );
            _productColor = new Lazy<IProductColorRepository>(
                () => new ProductColorRepository(_context)
            );
            _stock = new Lazy<IStockRepository>(() => new StockRepository(_context));
            _vendor = new Lazy<IBaseRepository<Vendor>>(() => new BaseRepository<Vendor>(_context));
        }

        // Update properties to use Lazy<T>.Value
        public IProductSizeRepository ProductSize => _productSize.Value;
        public IProductColorRepository ProductColor => _productColor.Value;
        public IStockRepository Stock => _stock.Value;
        public IBaseRepository<Vendor> Vendor => _vendor.Value;
    }
}
