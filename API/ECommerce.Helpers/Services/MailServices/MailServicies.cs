using System;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace ECommerce.Core.Services.MailServices
{
    public class MailServicies : IMailServicies
    {
        public async Task SendAsync(EmailDto emailDto)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailDto.MessageFrom);
                message.To.Add(new MailAddress(emailDto.Email));
                message.Subject = emailDto.Subject;
                message.Body = emailDto.Body;

                SmtpClient smtpClient = new SmtpClient(emailDto.Host, emailDto.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(
                    emailDto.UserName,
                    emailDto.Password
                );
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
