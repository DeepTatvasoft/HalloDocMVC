using HalloDoc.DataContext;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult PatientDashboard()
        {
            var temp = HttpContext.Session.GetInt32("Userid");
            IEnumerable<Request> data = _context.Requests.Where(u => u.Requestid == temp);
            return View(data);
        }
    }
}
