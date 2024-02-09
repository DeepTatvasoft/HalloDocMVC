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
        public IActionResult insert(PatientReqSubmit model)
        {
            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email);
            if (aspuser == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Email = model.Email;
                string username = model.FirstName + model.LastName;
                aspnetuser1.Username = username;
                aspnetuser1.Phonenumber = model.PhoneNumber;
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
            };

            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
            return RedirectToAction("patientlogin", "Home");
        }
    }
}
