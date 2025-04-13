using System.Threading.Tasks;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Vouchers.Services
{
    public interface IVoucherService
    {
        Task<BaseResponse> CreateAsync(CreateVoucherRequest request);
        Task<BaseResponse> DeleteAsync(DeleteVoucherRequest request);
        Task<BaseResponse> FindAsync(FindVoucherRequest request);
        Task<BaseResponse> GetAllAsync(GetAllVoucherRequest request);
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveVoucherRequest request);
        Task<BaseResponse> UpdateAsync(UpdateVoucherRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
