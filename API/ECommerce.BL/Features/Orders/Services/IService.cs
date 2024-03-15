using System.Threading.Tasks;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Orders.Services
{
    public interface IOrderService
    {
        Task<BaseResponse> CreateAsync(CreateOrderRequest request);
        Task<BaseResponse> DeleteAsync(DeleteOrderRequest request);
        Task<BaseResponse> FindAsync(FindOrderRequest request);
        Task<BaseResponse> GetAllAsync(GetAllOrderRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
