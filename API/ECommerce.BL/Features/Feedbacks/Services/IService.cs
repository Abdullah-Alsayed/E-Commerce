using System.Threading.Tasks;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Feedbacks.Services
{
    public interface IFeedbackService
    {
        Task<BaseResponse> CreateAsync(CreateFeedbackRequest request);
        Task<BaseResponse> DeleteAsync(DeleteFeedbackRequest request);
        Task<BaseResponse> FindAsync(FindFeedbackRequest request);
        Task<BaseResponse> GetAllAsync(GetAllFeedbackRequest request);
        Task<BaseResponse> UpdateAsync(UpdateFeedbackRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
