using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.OrderModule
{
    public class OrderModule : IOrderModule
    {
        private readonly ApplicationDbContext _context;

        public OrderModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IStatusRepository Status => _status ??= new StatusRepository(_context);
        private IStatusRepository _status;

        public IOrderRepository Order => _order ??= new OrderRepository(_context);
        private IOrderRepository _order;

        public IInvoiceRepository Invoice => _invoice ??= new InvoiceRepository(_context);
        private IInvoiceRepository _invoice;

        public IBaseRepository<Voucher> Voucher =>
            _voucher ??= new BaseRepository<Voucher>(_context);
        private IBaseRepository<Voucher> _voucher;

        public IBaseRepository<ShoppingCart> Cart =>
            _cart ??= new BaseRepository<ShoppingCart>(_context);
        private IBaseRepository<ShoppingCart> _cart;
    }
}
