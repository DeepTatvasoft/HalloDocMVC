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
                Requesttypeid = 1,
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
                Requesttypeid = 2,
                Relationname = model.FamRelation,
                Createddate = DateTime.Now,
                Status = 1
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
            string name = model.ConFirstName + model.ConLastName;
            Concierge concierge = new Concierge
            {
                Conciergename = name,
                Address = model.ConProperty,
                Street = model.ConStreet,
                City = model.ConCity,
                State = model.ConState,
                Zipcode = model.ConZipcode,
                Createddate = DateTime.Now,
                Regionid = 1
            };
            _context.Concierges.Add(concierge);
            Request req = new Request
            {
                Firstname = model.ConFirstName,
                Lastname = model.ConLastName,
                Phonenumber = model.ConPhonenumber,
                Email = model.ConEmail,
                Createddate= DateTime.Now,
                Requesttypeid = 3
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

        public IActionResult BusinessInfo(BusinessSubmit model)
        {
            Request req = new Request
            {
                Firstname = model.BusFirstname,
                Lastname = model.BusLastname,
                Phonenumber = model.BusPhonenumber,
                Email = model.BusEmail,
                Requesttypeid = 4,
                Status = 1
                
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

            Business bus = new Business
            {
                Name = model.BusFirstname + model.BusLastname,
                City = model.PatCity,
                Regionid = 1,
                Createddate = DateTime.Now,
                Zipcode = model.PatZipcode,
                Phonenumber = model.PatPhoneNumber,
                Businesstypeid = 1
            };
            _context.Businesses.Add(bus);
            _context.SaveChanges();
            return RedirectToAction("patientlogin", "Home");
        }
    }
}
