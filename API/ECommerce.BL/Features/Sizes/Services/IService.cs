using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Sizes.Dtos;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Sizes.Services
{
    public interface ISizeService
    {
        Task<BaseResponse> CreateAsync(CreateSizeRequest request);
        Task<BaseResponse> DeleteAsync(DeleteSizeRequest request);
        Task<BaseResponse> FindAsync(FindSizeRequest request);
        Task<BaseResponse<BaseGridResponse<List<SizeDto>>>> GetAllAsync(GetAllSizeRequest request);
        Task<BaseResponse> UpdateAsync(UpdateSizeRequest request);
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveSizeRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
