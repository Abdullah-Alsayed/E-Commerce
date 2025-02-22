using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.BLL.IRepository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<bool> AddUserToRoleAsync(AddUserToRoleRequest request);
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request);
        Task<int> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request);
        Task<int> UpdateUserClaimsAsync(UpdateUserClaimsRequest request);
        Task<Role> FindByName(string name);
        Task<Role> FindById(string id);
    }
}
