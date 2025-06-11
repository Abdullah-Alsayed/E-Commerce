using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.StockModule
{
    public interface IStockModule
    {
        IProductSizeRepository ProductSize { get; }
        IProductColorRepository ProductColor { get; }
        IStockRepository Stock { get; }
        IBaseRepository<Vendor> Vendor { get; }
    }
}
