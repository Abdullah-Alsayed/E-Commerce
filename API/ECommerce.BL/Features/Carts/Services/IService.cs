using System.Threading.Tasks;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Carts.Services
{
    public interface ICartService
    {
        Task<BaseResponse> CreateAsync(CreateCartRequest request);
        Task<BaseResponse> DeleteAsync(DeleteCartRequest request);
        Task<BaseResponse> GetUserAsync(GetUserCartRequest request);
        Task<BaseResponse> GetAllAsync(GetAllCartRequest request);
        Task<BaseResponse> UpdateAsync(UpdateCartRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
