using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ECommerce.Services.MailServices
{
    public class MailServicies : IMailServices
    {
        private readonly MailSettings _mailSettings;

        public MailServicies(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        //public async Task<bool> SendMail(string UserEmail , string UserName , string plainTextContent ,string htmlContent , string subject)
        //{
        //    var apiKey = "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";
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress(_mailSettings.Email, _mailSettings.DisplayName);
        //    var to = new EmailAddress(UserEmail, UserName);
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //    return await Task.FromResult(true);
        //}
        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();


            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        //}
    }
}
