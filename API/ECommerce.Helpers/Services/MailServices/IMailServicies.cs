using System.Threading.Tasks;

namespace ECommerce.Core.Services.MailServices
{
    public interface IMailServicies
    {
        Task SendAsync(EmailDto request);
    }
}
