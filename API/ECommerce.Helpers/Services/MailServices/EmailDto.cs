namespace ECommerce.Core.Services.MailServices
{
    public class EmailDto
    {
        public string Email { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string MessageFrom { get; set; } = "info@dinarcrm.com";
        public string Host { get; set; } = "smtp.sendgrid.net";
        public string Smtp { get; set; } = "smtp.sendgrid.net";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "apikey";
        public string Password { get; set; } = "SG.3Lm-COLIRYmKXx4Zj3FWeg.XwK7J2_z";
    }
}
