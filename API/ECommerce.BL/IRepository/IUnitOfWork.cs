using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using System;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IProdactRepository Prodact { get; }
        IUserRepository User { get; }
        IProdactImgRepository ProdactImg { get; }
        IBaseRepository<Category> Category { get; }
        IBaseRepository<SubCategory> SubCategory { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Unit> Unit { get; }
        IBaseRepository<Brand> Brand { get; }
        IBaseRepository<PromoCode> PromoCode { get; }
        IBaseRepository<Governorate> Governorate { get; }
        IBaseRepository<Area> Area { get; }
        IBaseRepository<Status> Status { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        IBaseRepository<Expense> Expense { get; }
        IBaseRepository<Review> Review { get; }
        IBaseRepository<Errors> Error { get; }
        IBaseRepository<Setting> Setting { get; }
        IBaseRepository<Slider> Slider { get; }

        INotificationRepository Notification { get; }

        Task<int> SaveAsync();
    }
}
