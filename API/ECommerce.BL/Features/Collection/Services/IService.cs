using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Collections.Dtos;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Collections.Services
{
    public interface ICollectionService
    {
        Task<BaseResponse> CreateAsync(CreateCollectionRequest request);
        Task<BaseResponse> DeleteAsync(DeleteCollectionRequest request);
        Task<BaseResponse> FindAsync(FindCollectionRequest request);
        Task<BaseResponse<BaseGridResponse<List<CollectionDto>>>> GetAllAsync(
            GetAllCollectionRequest request
        );
        Task<BaseResponse> UpdateAsync(UpdateCollectionRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveCollectionRequest request);
    }
}
