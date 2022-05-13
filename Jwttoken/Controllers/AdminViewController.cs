using Microsoft.AspNetCore.Mvc;

namespace Jwttoken.Controllers
{
    public class AdminViewController : Controller
    {
        public IActionResult AdminView()
        {
            return View();
        }
    }
}
