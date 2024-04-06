namespace ECommerce.Core.Services.MailServices
{
    public class EmailDto
    {
        public string Email { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string MessageFrom { get; set; } = "abdullahalsyead@gmail.com";
        public string Host { get; set; } = "smtp.gmail.com";
        public string Smtp { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "abdullahalsyead@gmail.com";
        public string Password { get; set; } = "fpdjneakwttwjymb";
    }
}
