using System.Threading.Tasks;
using ECommerce.BLL.Features.Area.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Area.Services
{
    public interface IAreaServices
    {
        Task<BaseResponse> CreateAsync(CreateAreaRequest request);
        Task<BaseResponse> DeleteAsync(DeleteAreaRequest request);
        Task<BaseResponse> FindAsync(FindAreaRequest request);
        Task<BaseResponse> GetAllAsync(GetAllAreaRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveAreaRequest request);
        Task<BaseResponse> UpdateAsync(UpdateAreaRequest request);
    }
}
