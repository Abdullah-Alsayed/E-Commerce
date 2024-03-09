using System.Threading.Tasks;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Brands.Services
{
    public interface IBrandService
    {
        Task<BaseResponse> CreateAsync(CreateBrandRequest request);
        Task<BaseResponse> DeleteAsync(DeleteBrandRequest request);
        Task<BaseResponse> FindAsync(FindBrandRequest request);
        Task<BaseResponse> GetAllAsync(GetAllBrandRequest request);
        Task<BaseResponse> UpdateAsync(UpdateBrandRequest request);
        Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveBrandRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
