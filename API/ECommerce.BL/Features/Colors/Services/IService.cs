using System.Threading.Tasks;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Colors.Services
{
    public interface IColorService
    {
        Task<BaseResponse> CreateAsync(CreateColorRequest request);
        Task<BaseResponse> DeleteAsync(DeleteColorRequest request);
        Task<BaseResponse> FindAsync(FindColorRequest request);
        Task<BaseResponse> GetAllAsync(GetAllColorRequest request);
        Task<BaseResponse> UpdateAsync(UpdateColorRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
