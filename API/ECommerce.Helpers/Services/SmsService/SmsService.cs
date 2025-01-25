using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ECommerce.Core.Services.SmsService
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _config;

        public SmsService(IConfiguration config) => _config = config;

        public async Task<MessageResource> SendMessage(string phoneNumber, string bodyMessage)
        {
            var accountSid = _config["Twilio:AccountSID"];
            var authToken = _config["Twilio:AuthToken"];

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: bodyMessage,
                from: _config["Twilio:TwilioPhoneNumber"],
                to: phoneNumber
            );

            return message;
        }
    }
}
