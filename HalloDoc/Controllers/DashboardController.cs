using HalloDoc.DataContext;
using HalloDoc.Models;
using HalloDoc.ViewModel;
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
            //User user = new User();
            if (HttpContext.Session.GetInt32("Userid")!=null)
            {
            int temp = (int)HttpContext.Session.GetInt32("Userid");
            var tempname = HttpContext.Session.GetString("Username");
            //IEnumerable<Request> data = _context.Requests.Where(u => u.Userid == temp);
            //return View(data);
            PatientDashboardedit dashedit = new PatientDashboardedit();
            var data = _context.Users.FirstOrDefault(u => u.Userid == temp);
            dashedit.User = data;
            DateTime tempDateTime = new DateTime(Convert.ToInt32(data.Intyear), Convert.ToInt32(data.Strmonth), (int)data.Intdate);
            dashedit.tempdate = tempDateTime;
            List<Requestwisefile> reqfile = (from m in _context.Requestwisefiles select m).ToList();
            dashedit.requestwisefiles = reqfile;
            var requestdata = _context.Requests.Where(u=>u.Userid == temp);
            dashedit.requests = requestdata.ToList();
            return View(dashedit);                
            }
            else
            {
                return RedirectToAction("patientlogin", "Home");
            }
        }
        public IActionResult editUser(PatientDashboardedit dashedit)
        {
            int id = (int)HttpContext.Session.GetInt32("Userid");
            int aspid = (int)HttpContext.Session.GetInt32("AspUserid");
            var user = _context.Users.FirstOrDefault(u=>u.Userid == id);
            user.Firstname = dashedit.User.Firstname;
            user.Lastname = dashedit.User.Lastname;
            string str = user.Firstname + " " + user.Lastname;
            HttpContext.Session.SetString("Username", str);
            user.Intdate = dashedit.tempdate.Day;
            user.Strmonth = (dashedit.tempdate.Month).ToString();
            user.Intyear = dashedit.tempdate.Year;
            user.Email = dashedit.User.Email;
            user.Mobile = dashedit.User.Mobile;
            user.Street = dashedit.User.Street;
            user.City = dashedit.User.City;
            user.State = dashedit.User.State;
            user.Zipcode = dashedit.User.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();

            var aspuser = _context.Aspnetusers.FirstOrDefault(u=>u.Id == aspid );
            aspuser.Username = str;
            _context.Aspnetusers.Update(aspuser);
            _context.SaveChanges();

            return RedirectToAction("PatientDashboard", "Dashboard");
        }
        public IActionResult ViewDocument()
        {
            return View();
        }
    }
}
