using Data.DataModels;
using HalloDoc.DataContext;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Diagnostics;
using System.Drawing;

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
        public IActionResult CreateAccount(string id)
        {
            PatientReqSubmit patientReqSubmit = new PatientReqSubmit();
            patientReqSubmit.reqclientid = id;
            return View(patientReqSubmit);
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
        public IActionResult newaccount(PatientReqSubmit model , string id)
        {
            if (model.Password == model.ConfirmPassword && model.Password != null)
            {
                Aspnetuser aspnetuser = new Aspnetuser();
                var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email);
                if (aspuser != null)
                {
                    TempData["error"] = "Email Already Exist";
                    return RedirectToAction("CreateAccount", "Home" , new { id = id });
                }
                int id2 = id.ElementAt(3);
                var reqc = _context.Requestclients.FirstOrDefault(u => u.Requestclientid == id2);
                aspnetuser.Email = model.Email;
                aspnetuser.Passwordhash = model.ConfirmPassword;
                aspnetuser.Username = reqc.Firstname + reqc.Lastname;
                aspnetuser.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Add(aspnetuser);
                User user = new User
                {
                    Firstname = reqc.Firstname,
                    Lastname = reqc.Lastname,
                    Email = model.Email,
                    Aspnetuser = aspnetuser,
                    Createdby = reqc.Firstname,
                    Intdate = reqc.Intdate,
                    Intyear = reqc.Intyear,
                    Strmonth = reqc.Strmonth,
                };
                _context.Users.Add(user);
                Request req = new Request
                {
                    Firstname = reqc.Firstname,
                    Lastname = reqc.Lastname,
                    Email = model.Email,
                    Phonenumber = reqc.Phonenumber,
                    Createddate = DateTime.Now,
                    Requesttypeid = 1,
                    Status = 1,
                    User = user,
                };
                _context.Requests.Add(req);
                Requestclient reqclient = new Requestclient
                {
                    Request = req,
                    Firstname = reqc.Firstname,
                    Lastname = reqc.Lastname,
                    Email = model.Email,
                    Phonenumber = reqc.Phonenumber,
                    Notes = reqc.Notes,
                    State = reqc.State,
                    City = reqc.City,
                    Street = reqc.Street,
                    Zipcode = reqc.Zipcode,
                    Intdate = reqc.Intdate,
                    Intyear = reqc.Intyear,
                    Strmonth = reqc.Strmonth,
                    Location = reqc.Location,
                    Address = reqc.Location + reqc.Street + reqc.City + reqc.State,
                };
                _context.Requestclients.Add(reqclient);
                _context.SaveChanges();
                TempData["success"] = "Your Account Created Sucessfuly";
                return RedirectToAction("patientlogin", "Home");
            }
            else
            {
                TempData["error"] = "Both passwords are different";
                return RedirectToAction("CreateAccount", "Home", new { id = id });

            }
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
                TempData["error"] = "Email or Password is Incorrect";
                return RedirectToAction("patientlogin", "Home");
            }
        }
        [HttpPost]
        public IActionResult changepassword(ResetPasswordVM vm, string id)
        {
            bool f = homefunction.changepassword(vm, id).Item1;
            if (f == true)
            {
                return RedirectToAction("patientlogin", "Home");
            }
            else
            {
                TempData["error"] = "Both passwords are different";
                return RedirectToAction("ResetPassword", "Home", new { id = id });
            }
        }

    }
}