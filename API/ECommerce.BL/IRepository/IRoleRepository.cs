using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<int> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request);
    }
}
