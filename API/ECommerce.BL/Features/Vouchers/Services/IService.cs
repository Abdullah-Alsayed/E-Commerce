using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Vouchers.Dtos;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Vouchers.Services
{
    public interface IVoucherService
    {
        Task<BaseResponse> CreateAsync(CreateVoucherRequest request);
        Task<BaseResponse> DeleteAsync(DeleteVoucherRequest request);
        Task<BaseResponse> FindAsync(FindVoucherRequest request);
        Task<BaseResponse<BaseGridResponse<List<VoucherDto>>>> GetAllAsync(
            GetAllVoucherRequest request
        );
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveVoucherRequest request);
        Task<BaseResponse> UpdateAsync(UpdateVoucherRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
