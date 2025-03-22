using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Users.Requests
{
    public record GetAllUserRequest : BaseGridRequest
    {
        public bool StaffOnly { get; set; } = false;
        public string RoleId { get; set; }
    }
}
