using System.Threading.Tasks;
using ECommerce.BLL.Features.Governorate.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Governorate.Services
{
    public interface IGovernorateServices
    {
        Task<BaseResponse> CreateAsync(CreateGovernorateRequest request);
        Task<BaseResponse> DeleteAsync(DeleteGovernorateRequest request);
        Task<BaseResponse> FindAsync(FindGovernorateRequest request);
        Task<BaseResponse> GetAllAsync(GetAllGovernorateRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveGovernorateRequest request);
        Task<BaseResponse> UpdateAsync(UpdateGovernorateRequest request);
    }
}
