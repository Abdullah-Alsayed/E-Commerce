using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.OrderModule
{
    public interface IOrderModule
    {
        IOrderRepository Order { get; }
        IInvoiceRepository Invoice { get; }
        IStatusRepository Status { get; }
        IBaseRepository<Voucher> Voucher { get; }
        IBaseRepository<ShoppingCart> Cart { get; }
    }
}
