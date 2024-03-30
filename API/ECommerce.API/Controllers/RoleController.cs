using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
