using Data.DataModels;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using Data.DataContext;

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
                int temp = (int)HttpContext.Session.GetInt32("Userid");
                var tempname = HttpContext.Session.GetString("Username");
                //IEnumerable<Request> data = _context.Requests.Where(u => u.Userid == temp);
                //return View(data);
                return View(dashboard.PatientDashboard(temp, tempname));
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
            HttpContext.Session.SetString("Username", dashboard.editUser(dashedit, id, aspid));
            return RedirectToAction("PatientDashboard", "Dashboard");
        }
        public IActionResult ViewDocument(int id)
        {
            int temp = id;
            int uid = (int)HttpContext.Session.GetInt32("Userid");
            var tempname = HttpContext.Session.GetString("Username");
            return View(dashboard.ViewDocument(temp, uid, tempname));
        }
        [HttpPost]
        public IActionResult DocUpload(PatientDashboardedit dashedit)
        {
            if (dashedit.Upload != null)
            {
                dashboard.AddPatientRequestWiseFile(dashedit.Upload, dashedit.reqid);
            }
            _context.SaveChanges();
            return RedirectToAction("ViewDocument", "Dashboard", new { id = dashedit.reqid });
        }

        [HttpPost]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(PatientDashboardedit dashedit)
        {
            var chk = Request.Form["checklist"].ToList();
            if (chk.Count == 0)
            {
                return NoContent();
            }
            return File(dashboard.DownloadFile(dashedit, chk).Item1, dashboard.DownloadFile(dashedit, chk).Item2, dashboard.DownloadFile(dashedit, chk).Item3, enableRangeProcessing: true);
        }

        [Route("SingleDownload")]
        public async Task<IActionResult> SingleDownload(int id)
        {
            return File(dashboard.FileDownload(id).Item1, dashboard.FileDownload(id).Item2, dashboard.FileDownload(id).Item2);
        }

        public IActionResult SubmitForMe()
        {
            return View();
        }
        public IActionResult SubmitForElse()
        {
            return View();
        }
        public IActionResult MeElse()
        {
            var chk = Request.Form["options-outlined"].ToList();
            if (chk.ElementAt(0) == "me")
            {
                return RedirectToAction("SubmitForMe", "Dashboard");
            }
            else if (chk.ElementAt(0) == "else")
            {
                return RedirectToAction("SubmitForElse", "Dashboard");

            }
            return NoContent();
        }
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
            sendEmail(email, "hello", "hello reset password https://localhost:44325/Home/ResetPassword/id=" + email + "");
            return RedirectToAction("patientlogin", "Home");
        }

        [HttpPost]
        public IActionResult SendAgreement(NewStateData modal)
        {
            sendEmail(modal.emaill, "Link for Agreement", "https://localhost:44325/admin/ReviewAgreement/" + modal.reqid + "");
            return NoContent();
        }
        public IActionResult SendLink(NewStateData modal)
        {
            string message = "Hello " + modal.firstname + modal.lastname;
            sendEmail(modal.emaill, " Link for Submit Request Screen", message + " https://localhost:44325/Home/Submitreqscreen");
            return RedirectToAction("AdminDashboard","Admin");
        }
    }
}

