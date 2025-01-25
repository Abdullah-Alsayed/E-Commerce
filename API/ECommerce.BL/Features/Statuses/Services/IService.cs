using System.Threading.Tasks;
using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Statuses.Services
{
    public interface IStatusService
    {
        Task<BaseResponse> CreateAsync(CreateStatusRequest request);
        Task<BaseResponse> DeleteAsync(DeleteStatusRequest request);
        Task<BaseResponse> FindAsync(FindStatusRequest request);
        Task<BaseResponse> GetAllAsync(GetAllStatusRequest request);
        Task<BaseResponse> UpdateAsync(UpdateStatusRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
