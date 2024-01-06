using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly Applicationdbcontext _Context;

        public NotificationRepository(Applicationdbcontext Context, IHostingEnvironment hosting)
            : base(Context, hosting) => _Context = Context;

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _ = await _Context.Set<Notification>().AddAsync(notification);
            return notification;
        }
    }
}
