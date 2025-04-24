using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Vendors.Dtos;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Vendors.Services
{
    public interface IVendorService
    {
        Task<BaseResponse> CreateAsync(CreateVendorRequest request);
        Task<BaseResponse> DeleteAsync(DeleteVendorRequest request);
        Task<BaseResponse> FindAsync(FindVendorRequest request);
        Task<BaseResponse<BaseGridResponse<List<VendorDto>>>> GetAllAsync(
            GetAllVendorRequest request
        );
        Task<BaseResponse> UpdateAsync(UpdateVendorRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveVendorRequest request);
    }
}
