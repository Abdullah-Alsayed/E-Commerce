using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.SubCategories.Dtos;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.SubCategories.Services
{
    public interface ISubCategoryService
    {
        Task<BaseResponse> CreateAsync(CreateSubCategoryRequest request);
        Task<BaseResponse> DeleteAsync(DeleteSubCategoryRequest request);
        Task<BaseResponse> FindAsync(FindSubCategoryRequest request);
        Task<BaseResponse<BaseGridResponse<List<SubCategoryDto>>>> GetAllAsync(
            GetAllSubCategoryRequest request
        );
        Task<BaseResponse> UpdateAsync(UpdateSubCategoryRequest request);
        Task<BaseResponse> ToggleActiveAsync(ToggleActiveSubCategoryRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
