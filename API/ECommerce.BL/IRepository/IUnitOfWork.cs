// Ignore Spelling: BLL

using System;
using System.Threading.Tasks;
using ECommerce.BLL.Repository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }
        IBaseRepository<SubCategory> SubCategory { get; }
        IBaseRepository<Governorate> Governorate { get; }
        IBaseRepository<Slider> Slider { get; }
        IBaseRepository<Voucher> Voucher { get; }
        IStockRepository Stock { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        INotificationRepository Notification { get; }
        IBaseRepository<History> History { get; }
        IBaseRepository<Feedback> Feedback { get; }
        IBaseRepository<ShoppingCart> Cart { get; }
        IOrderRepository Order { get; }
        IBaseRepository<Size> Size { get; }
        IBaseRepository<Tag> Tag { get; }
        IBaseRepository<Category> Category { get; }
        IErrorRepository ErrorLog { get; }
        IBaseRepository<Setting> Setting { get; }
        IBaseRepository<Expense> Expense { get; }
        IStatusRepository Status { get; }
        IInvoiceRepository Invoice { get; }
        IBaseRepository<ProductReview> Review { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Brand> Brand { get; }
        IProductRepository Product { get; }
        IBaseRepository<Unit> Unit { get; }
        IBaseRepository<Area> Area { get; }
        IBaseRepository<Vendor> Vendor { get; }
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IProductSizeRepository ProductSize { get; }
        IProductColorRepository ProductColor { get; }
        IBaseRepository<Booking> Booking { get; }

        Task<int> SaveAsync();
        Task<bool> IsDone(int modifyRows);
    }
}
