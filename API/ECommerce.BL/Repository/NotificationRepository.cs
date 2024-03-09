using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.BLL.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly Applicationdbcontext _Context;

        public NotificationRepository(Applicationdbcontext Context)
            : base(Context) => _Context = Context;

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            notification.Title = "";
            notification.Subject = "";
            notification.MessageAR = "";
            notification.MessageEN = "";

            _ = await AddaAync(notification);

            // Call Signal R
            return notification;
        }

        private string GenrateMessage()
        {
            return string.Empty;
        }
    }
}
