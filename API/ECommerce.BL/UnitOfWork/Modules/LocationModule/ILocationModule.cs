using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.LocationModule
{
    public interface ILocationModule
    {
        IBaseRepository<Governorate> Governorate { get; }
        IBaseRepository<Area> Area { get; }
    }
}
