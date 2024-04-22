using Common.Helper;
using Data.DataContext;
using Data.DataModels;
using DataAccess.ServiceRepository.IServiceRepository;
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
        private readonly IJwtRepository jwtRepository;
        public HomeController(ILogger<HomeController> logger, IHomeFunction homeFunction, ApplicationDbContext context, IJwtRepository jwtRepository)
        {
            _logger = logger;
            this.homefunction = homeFunction;
            _context = context;
            this.jwtRepository = jwtRepository;
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
            id = id.Substring(3);
            id = EncryptDecryptHelper.Decrypt(id);
            ResetPasswordVM vm = new ResetPasswordVM();
            vm.Email = id;
            return View(vm);
        }
        public IActionResult CreateAccount(string id)
        {
            id = id.Substring(3);
            PatientReqSubmit patientReqSubmit = new PatientReqSubmit();
            try
            {
                int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
                patientReqSubmit.reqclientid = id;
                patientReqSubmit.Email = _context.Requests.FirstOrDefault(u => u.Requestid == id2)!.Email;
                return View(patientReqSubmit);
            }
            catch
            {
                return RedirectToAction("Patientlogin","Home");
            }
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
        public IActionResult newaccount(PatientReqSubmit model, string id)
        {
            if (model.Password == model.ConfirmPassword && model.Password != null)
            {
                var aspuser = homefunction.getaspuser(model);
                if (aspuser != null)
                {
                    TempData["error"] = "Acount Already Exist";
                    return RedirectToAction("CreateAccount", "Home", new { id = "id=" + id });
                }
                homefunction.newaccount(model, id);
                TempData["success"] = "Your Account Created Sucessfuly";
                return RedirectToAction("patientlogin", "Home");
            }
            else
            {
                TempData["error"] = "Both passwords are different";
                return RedirectToAction("CreateAccount", "Home", new { id = "id=" + id });

            }
        }

        [HttpPost]
        public async Task<IActionResult> validate([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = homefunction.ValidateUser(aspNetUser).Item1;
            var user = homefunction.ValidateUser(aspNetUser).Item2;
            if (obj != null && user != null)
            {
                TempData["success"] = "User LogIn Successfully";
                HttpContext.Session.SetString("Username", obj.Username);
                HttpContext.Session.SetInt32("Userid", user.Userid);
                HttpContext.Session.SetInt32("AspUserid", (int)user.Aspnetuserid!);
                LoggedInPersonViewModel model = new LoggedInPersonViewModel();
                model.aspuserid = (int)user.Aspnetuserid;
                model.username = user.Firstname;
                model.role = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == user.Aspnetuserid.ToString())!.Roleid;
                //model.userid = _context.Users.FirstOrDefault(u => u.Aspnetuserid == id).Userid;
                Response.Cookies.Append("jwt", jwtRepository.GenerateJwtToken(model));
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
                return RedirectToAction("ResetPassword", "Home", new { id = "id=" + EncryptDecryptHelper.Encrypt(id) });
            }
        }

    }
}