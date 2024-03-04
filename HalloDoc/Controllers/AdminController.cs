﻿using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminFunction adminFunction;
        private readonly IDashboard dashboard;

        public AdminController(ApplicationDbContext context, IAdminFunction adminFunction, IDashboard dashboard)
        {
            _context = context;
            this.adminFunction = adminFunction;
            this.dashboard = dashboard;

        }
        public IActionResult adminlogin()
        {
            return View();
        }
        public IActionResult ActiveState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(4, 4, 5));
        }

        public IActionResult NewState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(1, 1, 1));
        }

        public IActionResult PendingState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(2, 2, 2));
        }
        public IActionResult TocloseState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(3, 7, 8));
        }
        public IActionResult UnpaidState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(9, 9, 9));
        }
        public IActionResult ConcludeState(string reqtypeid, string status, int regionid)
        {
            if (regionid != null && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status));
            }
            return View(adminFunction.AdminDashboarddata(6, 6, 6));
        }
        public IActionResult ViewCase(int id)
        {
            return PartialView("AdminLayout/_ViewCase", adminFunction.ViewCase(id));
        }
        public IActionResult ViewNotes(int reqid)
        {
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("Adminname") != null)
            {
                return View(adminFunction.AdminDashboarddata(1, 1, 1));

            }
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            (bool f, string adminname, int id) = adminFunction.loginadmin(aspNetUser);
            //string adminname = adminFunction.loginadmin(aspNetUser).Item2;
            if (f == false)
            {
                TempData["error"] = "Email or Password is Incorrect";
                return RedirectToAction("adminlogin", "admin");
            }
            else
            {
                HttpContext.Session.SetString("Adminname", adminname);
                HttpContext.Session.SetInt32("Adminid", id);
                if (HttpContext.Session.GetString("Adminname") != null)
                {
                    TempData["success"] = "Admin LogIn Successfully";
                }
                return RedirectToAction("Admindashboard", "Admin");
            }
        }
        public List<Physician> filterregion(string regionid)
        {
            return adminFunction.filterregion(regionid);
        }
        public IActionResult adminlogout()
        {
            HttpContext.Session.Remove("Adminname");
            TempData["Error"] = "Admin Logged Out Successfuly";
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult cancelcase(int reqid, int casetagid, string cancelnotes)
        {
            int id = (int)HttpContext.Session.GetInt32("Adminid");
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.cancelcase(reqid, casetagid, cancelnotes, adminname, id);
            return RedirectToAction("NewState", adminFunction.AdminDashboarddata(1, 1, 1));
        }
        public IActionResult assigncase(int reqid, int regid, int phyid, string Assignnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            int id = (int)HttpContext.Session.GetInt32("Adminid");
            adminFunction.assigncase(reqid, regid, phyid, Assignnotes, adminname, id);
            return RedirectToAction("NewState", adminFunction.AdminDashboarddata(1, 1, 1));
        }
        public IActionResult blockcase(int reqid, string Blocknotes)
        {
            adminFunction.blockcase(reqid, Blocknotes);
            return RedirectToAction("NewState", adminFunction.AdminDashboarddata(1, 1, 1));
        }
        [HttpPost]
        public IActionResult AdminNotesSaveChanges(int reqid, string adminnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.AdminNotesSaveChanges(reqid, adminnotes, adminname);
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }

        public IActionResult AdminuploadDoc(int reqid)
        {
            AdminviewDoc adminviewDoc = new AdminviewDoc();
            adminviewDoc.Username = _context.Requests.FirstOrDefault(u => u.Requestid == reqid).Firstname;
            adminviewDoc.ConfirmationNum = _context.Requests.FirstOrDefault(u => u.Requestid == reqid).Confirmationnumber;
            var reqfile = _context.Requestwisefiles.Where(u => u.Requestid == reqid).ToList();
            adminviewDoc.reqfile = reqfile;
            adminviewDoc.reqid = reqid;
            return PartialView("AdminLayout/_ViewDocument",adminviewDoc);
        }
        [HttpPost]
        public IActionResult DocUpload(List<IFormFile> myfile, string reqid)
        {
            //if (model.Upload != null)
            //{
            //    dashboard.AddPatientRequestWiseFile(model.Upload, model.reqid);
            //}
            //_context.SaveChanges();
            //AdminviewDoc adminviewDoc = new AdminviewDoc();
            //adminviewDoc.Username = _context.Requests.FirstOrDefault(u => u.Requestid == model.reqid).Firstname;
            //adminviewDoc.ConfirmationNum = _context.Requests.FirstOrDefault(u => u.Requestid == model.reqid).Confirmationnumber;
            //var reqfile = _context.Requestwisefiles.Where(u => u.Requestid == model.reqid).ToList();
            //adminviewDoc.reqfile = reqfile;
            //adminviewDoc.reqid = model.reqid;
            //return PartialView("AdminLayout/_ViewDocument", adminviewDoc);
            return NoContent();
        }
    }
}
