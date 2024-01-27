using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.BLL.Response;
using System.Threading.Tasks;

namespace ECommerce.BLL.Futures.Areas.Services
{
    public interface IAreaServices
    {
        Task<BaseResponse> CreateAsync(CreateAreaRequest request, string userId, string lang);
        Task<BaseResponse> DeleteAsync(DeleteAreaRequest request, string userId, string userName);
        Task<BaseResponse> FindAsync(FindAreaRequest request);
        Task<BaseResponse> GetAllAsync(GetAllAreaRequest request, string lang);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleAvtiveAsync(
            ToggleAvtiveAreaRequest request,
            string userId,
            string userName
        );
        Task<BaseResponse> UpdateAsync(UpdateAreaRequest request, string userId, string userName);
    }
}
