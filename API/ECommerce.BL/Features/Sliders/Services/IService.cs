using System.Threading.Tasks;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Sliders.Services
{
    public interface ISliderService
    {
        Task<BaseResponse> CreateAsync(CreateSliderRequest request);
        Task<BaseResponse> DeleteAsync(DeleteSliderRequest request);
        Task<BaseResponse> FindAsync(FindSliderRequest request);
        Task<BaseResponse> GetAllAsync(GetAllSliderRequest request);
        Task<BaseResponse> UpdateAsync(UpdateSliderRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
