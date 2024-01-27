using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.BLL.Response;
using System.Threading.Tasks;

namespace ECommerce.BLL.Futures.Governorates.Services
{
    public interface IGovernorateServices
    {
        Task<BaseResponse> CreateAsync(
            CreateGovernorateRequest request,
            string userId,
            string lang
        );
        Task<BaseResponse> DeleteAsync(
            DeleteGovernorateRequest request,
            string userId,
            string userName
        );
        Task<BaseResponse> FindAsync(FindGovernorateRequest request);
        Task<BaseResponse> GetAllAsync(GetAllGovernorateRequest request, string lang);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleAvtiveAsync(
            ToggleAvtiveGovernorateRequest request,
            string userId,
            string userName
        );
        Task<BaseResponse> UpdateAsync(
            UpdateGovernorateRequest request,
            string userId,
            string userName
        );
    }
}
