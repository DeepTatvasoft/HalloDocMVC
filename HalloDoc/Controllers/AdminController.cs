using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult adminlogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult loginadmin()
        {
            return View();
        }
    }
}
