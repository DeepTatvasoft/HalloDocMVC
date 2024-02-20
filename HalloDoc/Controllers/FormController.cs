using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModel;

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
            formSubmit.patientinfo(model);
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult familyinfo(FamilyFriendReqSubmit model)
        {
            formSubmit.familyinfo(model);
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

    }
}
