using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace ECommerce.Core.Services.MailServices
{
    public class MailServices : IMailServices
    {
        private readonly EmailSettings _emailSettings;

        public MailServices(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendAsync(EmailDto emailDto)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_emailSettings.From);
                message.To.Add(new MailAddress(emailDto.Email));
                message.Subject = emailDto.Subject;
                message.Body = emailDto.Body;
                SmtpClient smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(
                    _emailSettings.UserName,
                    _emailSettings.Password
                );
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}
