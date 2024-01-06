using ECommerce.DAL.Entity;
using System;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<SubCategory> SubCategory { get; }
        IBaseRepository<Governorate> Governorate { get; }
        IBaseRepository<SliderPhoto> SliderPhoto { get; }
        IBaseRepository<PromoCode> PromoCode { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        INotificationRepository Notification { get; }
        IProductPhotoRepository ProductPhoto { get; }
        IBaseRepository<Category> Category { get; }
        IBaseRepository<ErrorLog> ErrorLog { get; }
        IBaseRepository<Setting> Setting { get; }
        IBaseRepository<Expense> Expense { get; }
        IBaseRepository<Status> Status { get; }
        IBaseRepository<Review> Review { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Brand> Brand { get; }
        IProductRepository Product { get; }
        IBaseRepository<Unit> Unit { get; }
        IBaseRepository<Area> Area { get; }
        IUserRepository User { get; }

        Task<int> SaveAsync();
    }
}
