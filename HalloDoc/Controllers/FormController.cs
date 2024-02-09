using HalloDoc.DataContext;
using HalloDoc.Models;
using HalloDoc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class FormController : Controller
    {
        private readonly ILogger<FormController> _logger;
        private readonly ApplicationDbContext _context;
        public FormController(ILogger<FormController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult patientinfo(PatientReqSubmit model)
        {
            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email);
            if (aspuser == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Email = model.Email;
                string username = model.FirstName + model.LastName;
                aspnetuser1.Username = username;
                aspnetuser1.Phonenumber = model.PhoneNumber;
                aspnetuser1.Passwordhash = model.Password;
                aspnetuser1.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Add(aspnetuser1);
                aspuser = aspnetuser1;
            }

            User user = new User
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Mobile = model.PhoneNumber,
                Street = model.Street,
                City = model.City,
                State = model.State,
                Zipcode = model.Zipcode,
                Createdby = model.FirstName + model.LastName,
                Createddate = DateTime.Now,
                Aspnetuser = aspuser
            };

            _context.Users.Add(user);

            Request req = new Request
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                Createddate = DateTime.Now,
                Requesttypeid = 2,
                Status = 1,
                User = user
            };

            _context.Requests.Add(req);

            Requestclient reqclient = new Requestclient
            {
                Request = req,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                Notes = model.Symptoms,
                Intdate = model.DOB.Day,
                Intyear = model.DOB.Year,
                Strmonth = model.DOB.Month.ToString(),
                Location = model.Room
            };

            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
            return RedirectToAction("patientlogin", "Home");
        }

        [Route("/Form/patientinfo/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        public IActionResult familyinfo(FamilyFriendReqSubmit model)
        {
            Request req = new Request
            {
                Firstname = model.FamFirstName,
                Lastname = model.FamLastName,
                Phonenumber = model.FamMobile,
                Email = model.FamEmail,
                Relationname = model.FamRelation
            };
            _context.Requests.Add(req);
            Requestclient reqclient = new Requestclient
            {
                Notes = model.PatSymptoms,
                Firstname = model.PatFirstName,
                Lastname = model.PatLastName,
                Intdate = model.PatDOB.Day,
                Intyear = model.PatDOB.Year,
                Strmonth = model.PatDOB.Month.ToString(),
                Phonenumber = model.PatPhoneNumber,
                Street = model.PatStreet,
                City = model.PatCity,
                State = model.PatState,
                Zipcode = model.PatZipcode,
                Location = model.PatRoom,
                Request = req
            };
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
            return RedirectToAction("patientlogin", "Home");
        }

        public IActionResult ConciergeInfo(ConciergeSubmit model)
        {
            Request req = new Request();
            Requestclient reqclient = new Requestclient
            {
                Notes = model.PatSymptoms,
                Firstname = model.PatFirstName,
                Lastname = model.PatLastName,
                Intdate = model.PatDOB.Day,
                Intyear = model.PatDOB.Year,
                Strmonth = model.PatDOB.Month.ToString(),
                Phonenumber = model.PatPhoneNumber,
                Street = model.PatStreet,
                City = model.PatCity,
                State = model.PatState,
                Zipcode = model.PatZipcode,
                Location = model.PatRoom,
                Request = req
            };
            _context.Requestclients.Add(reqclient);
            return RedirectToAction("patientlogin", "Home");
        }
    }
}
