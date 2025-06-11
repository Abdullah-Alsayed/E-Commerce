using System.Threading.Tasks;
using ECommerce.BLL.Features.Settings.Dtos;
using ECommerce.BLL.Features.Settings.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Settings.Services
{
    public interface ISettingService
    {
        Task<BaseResponse<SettingDto>> GetAsync();
        Task<BaseResponse> UpdateAsync(UpdateSettingRequest request);
    }
}
