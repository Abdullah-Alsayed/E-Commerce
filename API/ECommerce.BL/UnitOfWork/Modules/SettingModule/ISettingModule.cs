using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.SettingModule
{
    public interface ISettingModule
    {
        IBaseRepository<Setting> Setting { get; }
        IBaseRepository<Expense> Expense { get; }
        IBaseRepository<Slider> Slider { get; }
    }
}
