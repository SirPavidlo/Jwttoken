using Microsoft.AspNetCore.Mvc;

namespace Jwttoken.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminView()
        {
            return View();
        }
    }
}
