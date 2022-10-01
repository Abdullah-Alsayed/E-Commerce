using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MailServices
{
    public interface IMailServices
    {
        Task SendEmailAsync(string mailTo, string subject, string body);
    }
}
