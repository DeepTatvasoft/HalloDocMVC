using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;
using Data.DataContext;

namespace HalloDoc.Controllers
{
    public class FormController : Controller
    {
        private readonly ILogger<FormController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IFormSubmit formSubmit;
        public FormController(ILogger<FormController> logger, ApplicationDbContext context, IFormSubmit formSubmit)
        {
            _logger = logger;
            _context = context;
            this.formSubmit = formSubmit;
        }
        public IActionResult patientinfo(PatientReqSubmit model)
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid");
            formSubmit.patientinfo(model,adminid);
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult familyinfo(FamilyFriendReqSubmit model)
        {
            (bool f,int id) = formSubmit.familyinfo(model);
            if (f == false)
            {
                var email = model.PatEmail;
                //int id = formSubmit.familyinfo(model).Item2;
                sendEmail(email, "hello", "hello reset password https://localhost:44325/Home/CreateAccount/id="+id+"");
            }
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult conciergeinfo(ConciergeSubmit model)
        {
            formSubmit.ConciergeInfo(model);
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult businessinfo(BusinessSubmit model)
        {
            formSubmit.BusinessInfo(model);
            return RedirectToAction("patientlogin", "Home");
        }
        [Route("/Form/patientreq/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
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
    }
}
