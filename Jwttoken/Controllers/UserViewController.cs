using Microsoft.AspNetCore.Mvc;

namespace Jwttoken.Controllers
{
    public class UserViewController : Controller
    {
        public IActionResult UserView()
        {
            return View();
        }
    }
}
