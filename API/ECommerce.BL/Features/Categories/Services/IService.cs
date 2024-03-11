using System.Threading.Tasks;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Categories.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse> CreateAsync(CreateCategoryRequest request);
        Task<BaseResponse> DeleteAsync(DeleteCategoryRequest request);
        Task<BaseResponse> FindAsync(FindCategoryRequest request);
        Task<BaseResponse> GetAllAsync(GetAllCategoryRequest request);
        Task<BaseResponse> UpdateAsync(UpdateCategoryRequest request);
        Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveCategoryRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
