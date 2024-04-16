using System.Threading.Tasks;
using ECommerce.BLL.Features.Bookings.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Bookings.Services
{
    public interface IBookingService
    {
        Task<BaseResponse> CreateAsync(CreateBookingRequest request);
        Task<BaseResponse> DeleteAsync(DeleteBookingRequest request);
        Task<BaseResponse> FindAsync(FindBookingRequest request);
        Task<BaseResponse> GetAllAsync(GetAllBookingRequest request);
        Task<BaseResponse> UpdateAsync(UpdateBookingRequest request);
        Task<BaseResponse> NotifyAsync(NotifyBookingRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
