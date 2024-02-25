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
        public IActionResult ActiveState()
        {
            return View(adminFunction.AdminDashboarddata(4, 4, 5));
        }


        public IActionResult NewState(string reqtypeid, string status)
        {
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
            NewStateData newStateData = new NewStateData();
            List<Request> req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requestid == id).ToList();
            newStateData.req = req;
            return View(newStateData);
        }

        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("Adminname") != null)
            {
                List<Request> req = (from m in _context.Requests select m).ToList();
                return View(req);
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
