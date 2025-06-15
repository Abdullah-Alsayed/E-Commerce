using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Tags.Dtos;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Tags.Services
{
    public interface IUnitService
    {
        Task<BaseResponse> CreateAsync(CreateUnitRequest request);
        Task<BaseResponse> DeleteAsync(DeleteUnitRequest request);
        Task<BaseResponse> FindAsync(FindUnitRequest request);
        Task<BaseResponse<BaseGridResponse<List<UnitDto>>>> GetAllAsync(GetAllUnitRequest request);
        Task<BaseResponse> UpdateAsync(UpdateUnitRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveUnitRequest request);
    }
}
