using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult ActiveState()
        {
            return View(adminFunction.AdminDashboarddata(4, 4, 5));
        }

        public IActionResult NewState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(1, 1, 1));
        }

        public IActionResult PendingState()
        {
            return View(adminFunction.AdminDashboarddata(2, 2, 2));
        }
        public IActionResult TocloseState()
        {
            return View(adminFunction.AdminDashboarddata(6, 6, 6));
        }
        public IActionResult UnpaidState()
        {
            return View(adminFunction.AdminDashboarddata(9, 9, 9));
        }
        public IActionResult ConcludeState()
        {
            return View(adminFunction.AdminDashboarddata(3, 7, 8));
        }
        public IActionResult ViewCase(int id)
        {
            return View(adminFunction.ViewCase(id));
        }

        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("Adminname") != null)
            {
                return View(adminFunction.AdminDashboarddata(1, 1, 1));

            }
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            bool f = adminFunction.loginadmin(aspNetUser).Item1;
            string adminname = adminFunction.loginadmin(aspNetUser).Item2;
            if (f == false)
            {
                TempData["error"] = "Email or Password is Incorrect";
                return RedirectToAction("adminlogin", "admin");
            }
            else
            {
                HttpContext.Session.SetString("Adminname", adminname);
                if (HttpContext.Session.GetString("Adminname") != null)
                {
                    TempData["success"] = "Admin LogIn Successfully";
                }
                return RedirectToAction("Admindashboard", "Admin");
            }
        }
        public IActionResult adminlogout()
        {
            HttpContext.Session.Remove("Adminname");
            TempData["Error"] = "Admin Logged Out Successfuly";
            return RedirectToAction("adminlogin", "Admin");
        }
    }
}
