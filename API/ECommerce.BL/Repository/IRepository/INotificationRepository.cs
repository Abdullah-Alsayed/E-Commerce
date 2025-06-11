using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.Repository.IRepository
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<Notification> AddNotificationAsync(Notification notification);
    }
}
