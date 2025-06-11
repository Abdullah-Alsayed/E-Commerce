using System.Threading.Tasks;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.BLL.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _Context;

        public NotificationRepository(ApplicationDbContext Context)
            : base(Context) => _Context = Context;

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            notification.Title = "";
            notification.Subject = "";
            notification.MessageAR = "";
            notification.MessageEN = "";

            _ = await AddAsync(notification, System.Guid.Empty);

            // Call Signal R
            return notification;
        }

        private string GenrateMessage()
        {
            return string.Empty;
        }
    }
}
