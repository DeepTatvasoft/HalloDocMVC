using Data.DataModels;
using HalloDoc.DataContext;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHomeFunction homefunction;
        public HomeController(ILogger<HomeController> logger, IHomeFunction homeFunction, ApplicationDbContext context)
        {
            _logger = logger;
            this.homefunction = homeFunction;
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
        public IActionResult ResetPassword(string id)
        {
            ResetPasswordVM vm = new ResetPasswordVM();
            vm.Email = id;
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult logout()
        {
            TempData["error"] = "User logged out Sucessfully";
            HttpContext.Session.Remove("Userid");
            return RedirectToAction("patientlogin", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> validate([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = homefunction.ValidateUser(aspNetUser).Item1;
            if (obj != null)
            {
                TempData["success"] = "User LogIn Successfully";
                HttpContext.Session.SetString("Username", obj.Username);
                var user = homefunction.ValidateUser(aspNetUser).Item2;
                HttpContext.Session.SetInt32("Userid", user.Userid);
                HttpContext.Session.SetInt32("AspUserid", (int)user.Aspnetuserid);
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            else
            {
                TempData["error"] = "Username or Password is Incorrect";
                return RedirectToAction("patientlogin", "Home");
            }
        }
        [HttpPost]
        public IActionResult changepassword(ResetPasswordVM vm, string id)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == id);
            if (vm.Password == vm.ConfirmPassword)
            {
                aspuser.Passwordhash = vm.Password;
                _context.Aspnetusers.Update(aspuser);
                _context.SaveChanges();
                return RedirectToAction("patientlogin", "Home");
            }
            else
            {
                TempData["error"] = "Both passwords are different";
                return RedirectToAction("ResetPassword", "Home", new { id = vm.Email });
            }
        }

    }
}