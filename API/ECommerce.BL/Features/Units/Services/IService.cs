using System.Threading.Tasks;
using ECommerce.BLL.Features.Units.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Units.Services
{
    public interface IUnitService
    {
        Task<BaseResponse> CreateAsync(CreateUnitRequest request);
        Task<BaseResponse> DeleteAsync(DeleteUnitRequest request);
        Task<BaseResponse> FindAsync(FindUnitRequest request);
        Task<BaseResponse> GetAllAsync(GetAllUnitRequest request);
        Task<BaseResponse> UpdateAsync(UpdateUnitRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
