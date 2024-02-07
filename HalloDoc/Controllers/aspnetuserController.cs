using HalloDoc.DataContext;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class aspnetuserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public aspnetuserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult IndexAspNetUser()
        {
            IEnumerable<Aspnetuser> data = _context.Aspnetusers;
            return View(data);
        }
    }
}
