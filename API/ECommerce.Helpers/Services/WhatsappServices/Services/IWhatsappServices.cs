using System.Threading.Tasks;

namespace ECommerce.Core.Services.WhatsappServices.Services
{
    public interface IWhatsappServices
    {
        Task<bool> SendMessage(WhatsappDto whatsappDto);
    }
}
