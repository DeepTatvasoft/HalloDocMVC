using Assignment.Models;
using Data.DataContext;
using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeFunction homeFunction;
        public HomeController(IHomeFunction homeFunction)
        {
            this.homeFunction = homeFunction;
        }

        public IActionResult Index()
        {
            return View(homeFunction.Index());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult SubmitData(StudentFormModal modal , string gender)
        {
           homeFunction.SubmitData(modal, gender);
            TempData["success"] = "Data Added Successfully";
            return RedirectToAction("Index");
        }
        public IActionResult AddStudent()
        {
            return PartialView("_AddStudent");
        }
        public IActionResult EditStudent(int id)
        {
            return PartialView("_EditStudent",homeFunction.EditStudent(id));
        }
        public IActionResult EditData(StudentFormModal modal , string gender)
        {
            homeFunction.EditData(modal, gender);
            TempData["success"] = "Data Updated Successfully";
            return RedirectToAction("Index");
        }
        public void DeleteData(int id)
        {
            homeFunction.DeleteData(id);
        }
    }
}