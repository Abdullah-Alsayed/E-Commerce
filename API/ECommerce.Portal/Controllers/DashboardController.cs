using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Portal.Controllers
{
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        public IActionResult Statistics()
        {
            return View();
        }
    }
}
