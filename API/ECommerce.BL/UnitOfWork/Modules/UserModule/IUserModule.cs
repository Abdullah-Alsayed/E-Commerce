using ECommerce.BLL.Repository.IRepository;

namespace ECommerce.BLL.UnitOfWork.Modules.UserModule
{
    public interface IUserModule
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
    }
}
