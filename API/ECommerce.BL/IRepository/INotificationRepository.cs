using ECommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface INotificationRepository:IBaseRepository<Notification>
    {
       Task<Notification> AddNotificationAsync(Notification notification);
    }
}
