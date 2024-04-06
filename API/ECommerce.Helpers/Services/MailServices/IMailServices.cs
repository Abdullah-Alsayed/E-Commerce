using System.Threading.Tasks;

namespace ECommerce.Core.Services.MailServices
{
    public interface IMailServices
    {
        Task<bool> SendAsync(EmailDto request);
    }
}
