using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace ECommerce.Core.Services.SmsService
{
    internal interface ISmsService
    {
        Task<MessageResource> SendMessage(string phoneNumber, string bodyMessage);
    }
}
