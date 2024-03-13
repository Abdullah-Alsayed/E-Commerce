using System.Threading.Tasks;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Vendors.Services
{
    public interface IVendorService
    {
        Task<BaseResponse> CreateAsync(CreateVendorRequest request);
        Task<BaseResponse> DeleteAsync(DeleteVendorRequest request);
        Task<BaseResponse> FindAsync(FindVendorRequest request);
        Task<BaseResponse> GetAllAsync(GetAllVendorRequest request);
        Task<BaseResponse> UpdateAsync(UpdateVendorRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
