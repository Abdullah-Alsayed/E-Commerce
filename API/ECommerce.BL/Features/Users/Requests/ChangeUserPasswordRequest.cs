using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Users.Requests
{
    public record ChangeUserPasswordRequest
    {
        public string ID { get; set; }
        public string NewPassword { get; set; }
    }
}
