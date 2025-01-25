namespace ECommerce.BLL.Features.Users.Requests
{
    public class ChangePasswordUserRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
