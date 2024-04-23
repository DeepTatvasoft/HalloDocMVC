using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;
using Data.DataContext;
using Common.Helper;
using Authorization = Services.Implementation.Authorization;

namespace HalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDashboard dashboard;
        public DashboardController(ApplicationDbContext context, IDashboard dashboard)
        {
            _context = context;
            this.dashboard = dashboard;
        }
        public IActionResult PatientDashboard()
        {
            //User user = new User();
            if (HttpContext.Session.GetInt32("Userid") != null)
            {
                int temp = (int)HttpContext.Session.GetInt32("Userid")!;
                return View(dashboard.PatientDashboard(temp));
            }
            else
            {
                return RedirectToAction("patientlogin", "Home");
            }
        }
        [Authorization("3")]
        public IActionResult editUser(PatientDashboardedit dashedit)
        {
            int id = (int)HttpContext.Session.GetInt32("Userid")!;
            HttpContext.Session.SetString("Username", dashboard.editUser(dashedit, id));
            return RedirectToAction("PatientDashboard", "Dashboard");
        }
        [Authorization("3")]
        public IActionResult ViewDocument(string id)
        {
            int temp = int.Parse(EncryptDecryptHelper.Decrypt(id));
            int uid = (int)HttpContext.Session.GetInt32("Userid")!;
            var tempname = HttpContext.Session.GetString("Username");
            return View(dashboard.ViewDocument(temp, uid, tempname!));
        }
        [Authorization("3")]
        [HttpPost]
        public IActionResult DocUpload(PatientDashboardedit dashedit)
        {
            if (dashedit.Upload != null)
            {
                dashboard.AddPatientRequestWiseFile(dashedit.Upload, dashedit.reqid);
            }
            _context.SaveChanges();
            return RedirectToAction("ViewDocument", "Dashboard", new { id = EncryptDecryptHelper.Encrypt(dashedit.reqid.ToString()) });
        }
        [Authorization("3")]
        [HttpPost]
        [Route("DownloadFile")]
        public IActionResult DownloadFile(PatientDashboardedit dashedit)
        {
            var chk = Request.Form["checklist"].ToList();
            if (chk.Count == 0)
            {
                return NoContent();
            }
            return File(dashboard.DownloadFile(dashedit, chk!).Item1, dashboard.DownloadFile(dashedit, chk!).Item2, dashboard.DownloadFile(dashedit, chk!).Item3, enableRangeProcessing: true);
        }
        [Authorization("3")]
        [Route("SingleDownload")]
        public IActionResult SingleDownload(int id)
        {
            return File(dashboard.FileDownload(id).Item1, dashboard.FileDownload(id).Item2, dashboard.FileDownload(id).Item2);
        }
        [Authorization("3")]
        public IActionResult SubmitForMe(int id)
        {
            return View(dashboard.SubmitForMe(id));
        }
        [Authorization("3")]
        public IActionResult SubmitForElse()
        {
            return View();
        }
        [Authorization("3")]
        public IActionResult MeElse()
        {
            var chk = Request.Form["options-outlined"].ToList();
            if (chk.ElementAt(0) == "me")
            {
                int uid = (int)HttpContext.Session.GetInt32("Userid")!;
                return RedirectToAction("SubmitForMe", new { id = uid });
            }
            else if (chk.ElementAt(0) == "else")
            {
                return RedirectToAction("SubmitForElse", "Dashboard");

            }
            return NoContent();
        }
        [Authorization("3")]
        public Task sendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.deeppatel@outlook.com";
            var password = "Deep2292002";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
        [HttpPost]
        public IActionResult send_mail()
        {
            var email = Request.Form["email"].ElementAt(0);
            var chkemail = dashboard.checkmail(email!);
            if (chkemail == false)
            {
                TempData["error"] = "Email Not Exist";
            }
            else
            {
                sendEmail(email!, "hello", "hello reset password https://localhost:44325/Home/ResetPassword/id=" + EncryptDecryptHelper.Encrypt(email!) + "");
                TempData["success"] = "Email Sent";
            }
            return RedirectToAction("patientlogin", "Home");
        }
        [HttpPost]
        public IActionResult sendmailAdmin()
        {
            var email = Request.Form["email"].ElementAt(0);
            var chkemail = dashboard.checkmailAdmin(email!);
            if (chkemail == false)
            {
                TempData["error"] = "Email Not Exist";
            }
            else
            {
                sendEmail(email!, "hello", "hello reset password https://localhost:44325/Admin/AdminResetPass/id=" + EncryptDecryptHelper.Encrypt(email!) + "");
                TempData["success"] = "Email Sent";
            }
            return RedirectToAction("adminlogin", "Admin");
        }
        [HttpPost]
        public IActionResult SendAgreement(NewStateData modal)
        {
            sendEmail(modal.emaill!, "Link for Agreement", "https://localhost:44325/admin/ReviewAgreement/" + EncryptDecryptHelper.Encrypt(modal.reqid.ToString()) + "");
            return NoContent();
        }
        public IActionResult SendLink(NewStateData modal)
        {
            string message = "Hello " + modal.firstname + modal.lastname;
            sendEmail(modal.emaill!, " Link for Submit Request Screen", message + " https://localhost:44325/Home/Submitreqscreen");
            TempData["success"] = "Email Sent";
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}

