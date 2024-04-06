using System.Threading.Tasks;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Users.Services
{
    public interface IUserService
    {
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordUserRequest request);
        Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordUserRequest request);
        Task<BaseResponse> LoginAsync(LoginRequest request);
        Task<BaseResponse> LogOfAsync();

        //Task<BaseResponse> CreateAsync(CreateUserRequest request);
        //Task<BaseResponse> DeleteAsync(DeleteUserRequest request);
        //Task<BaseResponse> FindAsync(FindUserRequest request);
        //Task<BaseResponse> GetAllAsync(GetAllUserRequest request);
        //Task<BaseResponse> UpdateAsync(UpdateUserRequest request);
        //Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> RegisterAsync(CreateUserRequest request);
        Task<BaseResponse> UserInfoAsync();
    }
}
