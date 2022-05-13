using Microsoft.AspNetCore.Mvc;

namespace Jwttoken.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserView()
        {
            return View();
        }
    }
}
