using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly Applicationdbcontext _Context;

        public NotificationRepository(Applicationdbcontext Context , IHostingEnvironment hosting) : base(Context, hosting)
        {
            _Context = Context;
        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
             await _Context.Notifications.AddAsync(notification);
            return notification;
        }
    }
}
