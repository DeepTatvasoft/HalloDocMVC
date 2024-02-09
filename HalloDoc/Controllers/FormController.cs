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
            return RedirectToAction("patientlogin", "Home");
        }
    }
}
