using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Tags.Dtos;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Tags.Services
{
    public interface ITagService
    {
        Task<BaseResponse> CreateAsync(CreateTagRequest request);
        Task<BaseResponse> DeleteAsync(DeleteTagRequest request);
        Task<BaseResponse> FindAsync(FindTagRequest request);
        Task<BaseResponse<BaseGridResponse<List<TagDto>>>> GetAllAsync(GetAllTagRequest request);
        Task<BaseResponse> UpdateAsync(UpdateTagRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveTagRequest request);
    }
}
