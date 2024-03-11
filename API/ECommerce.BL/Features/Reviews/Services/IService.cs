using System.Threading.Tasks;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Reviews.Services
{
    public interface IReviewService
    {
        Task<BaseResponse> CreateAsync(CreateReviewRequest request);
        Task<BaseResponse> DeleteAsync(DeleteReviewRequest request);
        Task<BaseResponse> FindAsync(FindReviewRequest request);
        Task<BaseResponse> GetAllAsync(GetAllReviewRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> UpdateAsync(UpdateReviewRequest request);
    }
}
