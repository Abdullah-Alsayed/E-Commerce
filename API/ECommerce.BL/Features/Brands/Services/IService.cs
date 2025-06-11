using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Brands.Services
{
    public interface IBrandService
    {
        Task<BaseResponse> CreateAsync(CreateBrandRequest request);
        Task<BaseResponse> DeleteAsync(DeleteBrandRequest request);
        Task<BaseResponse> FindAsync(FindBrandRequest request);
        Task<BaseResponse<BaseGridResponse<List<BrandDto>>>> GetAllAsync(
            GetAllBrandRequest request
        );
        Task<BaseResponse> UpdateAsync(UpdateBrandRequest request);
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveBrandRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
