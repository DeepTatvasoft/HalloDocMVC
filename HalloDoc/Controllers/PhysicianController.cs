using Data.DataContext;
using Data.DataModels;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;

namespace HalloDoc.Controllers
{
    [Authorization("2")]
    public class PhysicianController : Controller
    {
        private readonly IPhysicianFunction physicianFunction;
        private readonly IAdminFunction adminFunction;
        private readonly ApplicationDbContext _context;
        public PhysicianController(ApplicationDbContext context, IPhysicianFunction physicianFunction, IAdminFunction adminFunction)
        {
            _context = context;
            this.physicianFunction = physicianFunction;
            this.adminFunction = adminFunction;
        }
        public IActionResult PhysicianDashboard()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (HttpContext.Session.GetString("physicianname") != null)
            {
                return View(physicianFunction.AdminDashboard(phyid));
            }
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult ActiveStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(4, currentPage, phyid, searchkey));
        }
        public IActionResult NewStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(1, currentPage, phyid, searchkey));
        }
        public IActionResult PendingStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(2, currentPage, phyid, searchkey));
        }
        public IActionResult ConcludeStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(6, currentPage, phyid, searchkey));
        }
        public IActionResult PhysicianAccept(int id)
        {
            physicianFunction.PhysicianAccept(id);
            TempData["success"] = "Accepted";
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult Transfer(int reqid, string Assignnotes)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            physicianFunction.Transfer(reqid, Assignnotes, phyid);
            return RedirectToAction("PhysicianDashboard");
        }

        public IActionResult EncounterBtn(NewStateData modal, string[] encounterchk)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            physicianFunction.EncounterBtn(modal,encounterchk,phyid);
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult HouseCallBtn(int id)
        {
            physicianFunction.HouseCallBtn(id);
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult EncounterForm(int id)
        {
            return View(physicianFunction.EncounterForm(id));
        }
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            physicianFunction.EncounterFormSubmit(model);
            TempData["success"] = "Data Saved Successfuly";
            return RedirectToAction("EncounterForm", new { id = model.RequestId });
        }
        public IActionResult EncounterFormFinalize(EncounterFormViewModel modal)
        {
            physicianFunction.EncounterFormFinalize(modal);
            TempData["success"] = "Encounter form finalized successfully";
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult ConcludeCare(int id)
        {
            return View(adminFunction.AdminuploadDoc(id));
        }
        public IActionResult ConcludeCareBtn(AdminviewDoc modal)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            var check = physicianFunction.ConcludeCareBtn(modal,phyid);
            if (check == false)
            {
                TempData["error"] = "You cannot conclude the case until encounter form is Finalized";
                return RedirectToAction("ConcludeCare", new { id = modal.reqid });
            }
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult DownloadEncounter(NewStateData modal)
        {
            return physicianFunction.DownloadEncounter(modal.reqid);
        }
        public bool CheckSession()
        {
            if (HttpContext.Session.GetString("physicianname") != null)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public IActionResult PhysicianNotesSaveChanges(int reqid, string physiciannotes)
        {
            string phyname = HttpContext.Session.GetString("physicianname")!;
            physicianFunction.PhysicianNotesSaveChanges(reqid, physiciannotes, phyname);
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }

        public IActionResult PhysicianProfile()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            return View(adminFunction.EditPhysician(phyid));
        }
        public IActionResult PhysicianSchedule()
        {
            return View(adminFunction.Scheduling());
        }
        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            return PartialView("AdminLayout/_MonthWise", physicianFunction.LoadSchedulingPartial(date,regionid,phyid));
        }
        public IActionResult AddShift(Scheduling model)
        {
            string phyname = HttpContext.Session.GetString("physicianname")!;
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            model.physicianid = phyid;
            if (model.starttime > model.endtime)
            {
                TempData["error"] = "Starttime Must be Less than Endtime";
                return RedirectToAction("Scheduling");
            }
            string adminname = HttpContext.Session.GetString("Adminname")!;
            var chk = Request.Form["repeatdays"].ToList();
            bool f = adminFunction.AddShift(model, phyname, chk!);
            if (f == false)
            {
                TempData["error"] = "Shift is already assigned in this time";
            }
            return RedirectToAction("PhysicianSchedule");
        }
    }
}
