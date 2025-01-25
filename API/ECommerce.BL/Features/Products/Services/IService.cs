using System.Threading.Tasks;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Products.Services
{
    public interface IProductService
    {
        Task<BaseResponse> CreateAsync(CreateProductRequest request);
        Task<BaseResponse> DeleteAsync(DeleteProductRequest request);
        Task<BaseResponse> FindAsync(FindProductRequest request);
        Task<BaseResponse> GetAllAsync(GetAllProductRequest request);
        Task<BaseResponse> UpdateAsync(UpdateProductRequest request);
        Task<BaseResponse> ToggleActivesAsync(ToggleAvtiveProductRequest request);
        Task<BaseResponse> GetProductItems(GetProductItemsRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
