using System.Threading.Tasks;
using ECommerce.Core.Services.WhatsappServices;
using ECommerce.Core.Services.WhatsappServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class SenderController : Controller
    {
        readonly IWhatsappServices _services;

        public SenderController(IWhatsappServices services) => _services = services;

        [HttpPost]
        public async Task<IActionResult> SendWhatsAppMessage()
        {
            var result = await _services.SendMessage(
                new WhatsappDto
                {
                    Language = "en_US",
                    Number = "201118690648",
                    Template = "hello_world"
                }
            );
            return Ok(result);
        }
    }
}
