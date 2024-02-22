using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminFunction adminFunction;
        public AdminController(ApplicationDbContext context, IAdminFunction adminFunction)
        {
            _context = context;
            this.adminFunction = adminFunction;
        }
        public IActionResult adminlogin()
        {
            return View();
        }
        public IActionResult AdminDashboard()
        {
            return View(adminFunction.AdminDashboarddata());
        }
        public IActionResult loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            bool f = adminFunction.loginadmin(aspNetUser);          
            if (f == false)
            {
                TempData["error"] = "Email or Password is Incorrect";
                return RedirectToAction("adminlogin", "admin");
            }
            else
            {
                TempData["success"] = "Admin LogIn Successfully";
                return RedirectToAction("Admindashboard", "Admin");
            }
        }
    }
}
