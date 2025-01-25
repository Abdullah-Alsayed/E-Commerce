namespace ECommerce.BLL.Features.Users.Requests
{
    public class ResetPasswordUserRequest
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
