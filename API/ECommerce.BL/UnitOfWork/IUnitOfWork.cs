// Ignore Spelling: BLL
using System;
using System.Threading.Tasks;
using ECommerce.BLL.UnitOfWork.Modules.ContentModule;
using ECommerce.BLL.UnitOfWork.Modules.LocationModule;
using ECommerce.BLL.UnitOfWork.Modules.OrderModule;
using ECommerce.BLL.UnitOfWork.Modules.ProductModule;
using ECommerce.BLL.UnitOfWork.Modules.SettingModule;
using ECommerce.BLL.UnitOfWork.Modules.StockModule;
using ECommerce.BLL.UnitOfWork.Modules.UserModule;
using ECommerce.DAL;

namespace ECommerce.BLL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        IProductModule ProductModule { get; }
        IOrderModule OrderModule { get; }
        IUserModule UserModule { get; }
        ISettingModule SettingModule { get; }
        ILocationModule LocationModule { get; }
        IContentModule ContentModule { get; }
        IStockModule StockModule { get; }

        Task<int> SaveAsync();
        Task<bool> IsDone(int modifyRows);
    }
}
