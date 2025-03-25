using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Roles.Services
{
    public interface IRoleService
    {
        Task<BaseResponse> CreateAsync(CreateRoleRequest request);
        Task<BaseResponse> DeleteAsync(DeleteRoleRequest request);
        Task<BaseResponse> FindAsync(FindRoleRequest request);
        Task<BaseResponse<BaseGridResponse<List<RoleDto>>>> GetAllAsync(GetAllRoleRequest request);
        Task<BaseResponse> UpdateAsync(UpdateRoleRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request);
        Task<BaseResponse> UpdateUserClaimsAsync(UpdateUserClaimsRequest request);
        Task<BaseResponse> AddUserToRoleAsync(AddUserToRoleRequest request);
        Task<BaseResponse> UpdateUserRoleAsync(UpdateUserRoleRequest request);
        Task<BaseResponse<List<RoleClaimsDto>>> GetClaimsAsync(BaseRequest request);
    }
}
