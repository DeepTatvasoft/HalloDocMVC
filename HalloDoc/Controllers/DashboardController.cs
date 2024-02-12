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
            IEnumerable<Request> data = _context.Requests;
            return View(data);
        }
    }
}
