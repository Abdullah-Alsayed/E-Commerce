using System.Threading.Tasks;
using ECommerce.BLL.Features.Settings.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Settings.Services
{
    public interface ISettingService
    {
        Task<BaseResponse> GetAsync();
        Task<BaseResponse> UpdateAsync(UpdateSettingRequest request);
    }
}
