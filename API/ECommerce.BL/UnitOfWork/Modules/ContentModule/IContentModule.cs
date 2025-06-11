using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ContentModule
{
    public interface IContentModule
    {
        IBaseRepository<Feedback> Feedback { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        IBaseRepository<History> History { get; }
        INotificationRepository Notification { get; }
        IErrorRepository ErrorLog { get; }
    }
}
