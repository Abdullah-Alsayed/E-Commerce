using System;
using System.Threading.Tasks;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        Applicationdbcontext Context { get; }
        IBaseRepository<SubCategory> SubCategory { get; }
        IBaseRepository<Governorate> Governorate { get; }
        IBaseRepository<Slider> Slider { get; }
        IBaseRepository<Voucher> Voucher { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        INotificationRepository Notification { get; }
        IBaseRepository<History> History { get; }
        IBaseRepository<Feedback> Feedback { get; }
        IBaseRepository<ShoppingCart> Cart { get; }
        IBaseRepository<Order> Order { get; }
        IBaseRepository<Size> Size { get; }
        IBaseRepository<Category> Category { get; }
        IErrorRepository ErrorLog { get; }
        IBaseRepository<Setting> Setting { get; }
        IBaseRepository<Expense> Expense { get; }
        IBaseRepository<Status> Status { get; }
        IBaseRepository<Invoice> Invoice { get; }
        IBaseRepository<ProductReview> Review { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Brand> Brand { get; }
        IProductRepository Product { get; }
        IBaseRepository<Unit> Unit { get; }
        IBaseRepository<Area> Area { get; }
        IBaseRepository<Vendor> Vendor { get; }
        IUserRepository User { get; }

        Task<int> SaveAsync();
        Task<bool> IsDone(int modifyRows);
    }
}
