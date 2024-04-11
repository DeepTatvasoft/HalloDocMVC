using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class PhysicianController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
