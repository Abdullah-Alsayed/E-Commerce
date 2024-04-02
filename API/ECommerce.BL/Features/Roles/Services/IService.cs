using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Roles.Services
{
    public interface IRoleService
    {
        Task<BaseResponse> CreateAsync(CreateRoleRequest request);
        Task<BaseResponse> DeleteAsync(DeleteRoleRequest request);
        Task<BaseResponse> FindAsync(FindRoleRequest request);
        Task<BaseResponse> GetAllAsync(GetAllRoleRequest request);
        Task<BaseResponse> UpdateAsync(UpdateRoleRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request);
    }
}
