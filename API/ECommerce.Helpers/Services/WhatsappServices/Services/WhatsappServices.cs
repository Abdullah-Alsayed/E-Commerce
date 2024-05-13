using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using static ECommerce.Core.Constants;

namespace ECommerce.Core.Services.WhatsappServices.Services
{
    public class WhatsappServices : IWhatsappServices
    {
        readonly WhatsappSettings _settings;

        public WhatsappServices(IOptions<WhatsappSettings> settings) => _settings = settings.Value;

        public async Task<bool> SendMessage(WhatsappDto whatsappDto)
        {
            using HttpClient httpClient = new();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                _settings.Token
            );

            WhatsAppRequest body =
                new()
                {
                    to = whatsappDto.Number,
                    template = new WhatsAppTemplate
                    {
                        name = whatsappDto.Template,
                        language = new WhatsAppLanguage { code = whatsappDto.Language },
                        components = whatsappDto.components
                    }
                };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                new Uri(_settings.ApiUrl),
                body
            );

            return response.IsSuccessStatusCode;
        }
    }
}
