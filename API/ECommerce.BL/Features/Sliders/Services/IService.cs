using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Features.Sliders.Dtos;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Sliders.Services
{
    public interface ISliderService
    {
        Task<BaseResponse> CreateAsync(CreateSliderRequest request);
        Task<BaseResponse> DeleteAsync(DeleteSliderRequest request);
        Task<BaseResponse> FindAsync(FindSliderRequest request);
        Task<BaseResponse<BaseGridResponse<List<SliderDto>>>> GetAllAsync(
            GetAllSliderRequest request
        );
        Task<BaseResponse> UpdateAsync(UpdateSliderRequest request);

        Task<BaseResponse> ToggleActiveAsync(ToggleActiveSliderRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
