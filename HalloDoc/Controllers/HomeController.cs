using HalloDoc.DataContext;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult patientsite()
        {
            return View();
        }

        public IActionResult SubmitReqScreen()
        {
            return View();
        }
        public IActionResult patientlogin()
        {
            return View();
        }
        public IActionResult patientreset()
        {
            return View();
        }
        public IActionResult patientreq()
        {
            return View();
        }
        public IActionResult FamilyFriendReq()
        {
            return View();
        }
        public IActionResult ConciergeReq()
        {
            return View();
        }
        public IActionResult BusinessInfo()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult insert(User obj)
        {
            obj.Createddate = DateTime.Now;
            obj.Createdby = "asd";
            _context.Users.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("patientlogin","Home");
        }

        [HttpPost]
        public async Task<IActionResult> validate([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj != null)
            {
                TempData["success"] = "User LogIn Successfully";
                return RedirectToAction("SubmitReqScreen", "Home");
            }
            else
            {
                TempData["error"] = "Username or Password is Incorrect";
                return RedirectToAction("patientlogin", "Home");
            }
        }
    }
}