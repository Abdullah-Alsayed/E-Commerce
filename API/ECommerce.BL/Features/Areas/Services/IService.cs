using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Areas.Dtos;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Areas.Services
{
    public interface IAreaService
    {
        Task<BaseResponse> CreateAsync(CreateAreaRequest request);
        Task<BaseResponse> DeleteAsync(DeleteAreaRequest request);
        Task<BaseResponse> FindAsync(FindAreaRequest request);
        Task<BaseResponse<BaseGridResponse<List<AreaDto>>>> GetAllAsync(GetAllAreaRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveAreaRequest request);
        Task<BaseResponse> UpdateAsync(UpdateAreaRequest request);
    }
}
