using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<bool> AddUserToRoleAsync(AddUserToRoleRequest request);
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request);
        Task<int> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request);
        Task<int> UpdateUserClaimsAsync(UpdateUserClaimsRequest request);
    }
}
