using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using System;

namespace ECommerce.BLL.UnitOfWork.Modules.OrderModule
{
    public class OrderModule : IOrderModule
    {
        private readonly ApplicationDbContext _context;

        private readonly Lazy<IStatusRepository> _status;
        private readonly Lazy<IOrderRepository> _order;
        private readonly Lazy<IInvoiceRepository> _invoice;
        private readonly Lazy<IBaseRepository<Voucher>> _voucher;
        private readonly Lazy<IBaseRepository<ShoppingCart>> _cart;

        public OrderModule(ApplicationDbContext context)
        {
            _context = context;

            _status = new Lazy<IStatusRepository>(() => new StatusRepository(_context));
            _order = new Lazy<IOrderRepository>(() => new OrderRepository(_context));
            _invoice = new Lazy<IInvoiceRepository>(() => new InvoiceRepository(_context));
            _voucher = new Lazy<IBaseRepository<Voucher>>(() => new BaseRepository<Voucher>(_context));
            _cart = new Lazy<IBaseRepository<ShoppingCart>>(() => new BaseRepository<ShoppingCart>(_context));
        }

        public IStatusRepository Status => _status.Value;
        public IOrderRepository Order => _order.Value;
        public IInvoiceRepository Invoice => _invoice.Value;
        public IBaseRepository<Voucher> Voucher => _voucher.Value;
        public IBaseRepository<ShoppingCart> Cart => _cart.Value;
    }
}
