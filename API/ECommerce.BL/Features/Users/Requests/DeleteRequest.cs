using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Users.Requests
{
    public record DeleteUserRequest : BaseRequest
    {
        public string Token { get; set; }
    }
}
