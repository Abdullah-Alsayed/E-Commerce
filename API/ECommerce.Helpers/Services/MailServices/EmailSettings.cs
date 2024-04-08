namespace ECommerce.Core.Services.MailServices
{
    public class EmailSettings
    {
        public string From { get; set; }
        public string Host { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
