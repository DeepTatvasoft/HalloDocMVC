using Common.Helper;
using Data.DataModels;
using DataAccess.ServiceRepository.IServiceRepository;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeFunction homefunction;
        private readonly IAdminFunction adminfunction;
        private readonly IJwtRepository jwtRepository;
        public HomeController(IHomeFunction homeFunction, IJwtRepository jwtRepository, IAdminFunction adminfunction)
        {
            this.homefunction = homeFunction;
            this.jwtRepository = jwtRepository;
            this.adminfunction = adminfunction;
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
            try
            {
                return View(homefunction.CreateAccount(id));
            }
            catch
            {
                return RedirectToAction("Patientlogin", "Home");
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
                model.role = adminfunction.getrole(model.aspuserid);
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
        [HttpPost]
        public IActionResult changepasswordAdmin(ResetPasswordVM vm, string id)
        {
            bool f = homefunction.changepassword(vm, id).Item1;
            if (f == true)
            {
                return RedirectToAction("Adminlogin", "Admin");
            }
            else
            {
                TempData["error"] = "Both passwords are different";
                return RedirectToAction("AdminResetPass", "Admin", new { id = "id=" + EncryptDecryptHelper.Encrypt(id) });
            }
        }
    }
}