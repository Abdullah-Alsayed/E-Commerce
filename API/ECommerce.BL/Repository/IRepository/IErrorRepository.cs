using System;
using System.Threading.Tasks;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Repository.IRepository
{
    public interface IErrorRepository : IBaseRepository<ErrorLog>
    {
        Task ErrorLog(Exception ex, OperationTypeEnum action, EntitiesEnum entity);
    }
}
