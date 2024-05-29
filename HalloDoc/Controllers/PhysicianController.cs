using Data.DataContext;
using Data.DataModels;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly IPhysicianFunction physicianFunction;
        private readonly IAdminFunction adminFunction;
        public PhysicianController(IPhysicianFunction physicianFunction, IAdminFunction adminFunction)
        {
            this.physicianFunction = physicianFunction;
            this.adminFunction = adminFunction;
        }
        [Authorization("2")]
        public IActionResult PhysicianDashboard()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (HttpContext.Session.GetString("physicianname") != null)
            {
                return View(physicianFunction.AdminDashboard(phyid));
            }
            return RedirectToAction("adminlogin", "Admin");
        }
        [Authorization("2")]
        public IActionResult ActiveStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(4, currentPage, phyid, searchkey));
        }
        [Authorization("2")]
        public IActionResult NewStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(1, currentPage, phyid, searchkey));
        }
        [Authorization("2")]
        public IActionResult PendingStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(2, currentPage, phyid, searchkey));
        }
        [Authorization("2")]
        public IActionResult ConcludeStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(6, currentPage, phyid, searchkey));
        }
        [Authorization("2")]
        public void PhysicianAccept(int id)
        {
            physicianFunction.PhysicianAccept(id);
        }
        [Authorization("2")]
        public IActionResult Transfer(int reqid, string Assignnotes)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            physicianFunction.Transfer(reqid, Assignnotes, phyid);
            return RedirectToAction("PhysicianDashboard");
        }
        [Authorization("2")]
        public IActionResult EncounterBtn(NewStateData modal, string[] encounterchk)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            physicianFunction.EncounterBtn(modal, encounterchk, phyid);
            return RedirectToAction("PhysicianDashboard");
        }
        [Authorization("2")]
        public IActionResult HouseCallBtn(int id)
        {
            physicianFunction.HouseCallBtn(id);
            return RedirectToAction("PhysicianDashboard");
        }
        [Authorization("1,2")]
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
        [Authorization("2")]
        public IActionResult ConcludeCare(int id)
        {
            return View(adminFunction.AdminuploadDoc(id));
        }
        [Authorization("2")]
        public IActionResult ConcludeCareBtn(AdminviewDoc modal)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            var check = physicianFunction.ConcludeCareBtn(modal, phyid);
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
        [Authorization("2")]
        [HttpPost]
        public IActionResult PhysicianNotesSaveChanges(int reqid, string physiciannotes)
        {
            string phyname = HttpContext.Session.GetString("physicianname")!;
            physicianFunction.PhysicianNotesSaveChanges(reqid, physiciannotes, phyname);
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }
        [Authorization("2")]
        public IActionResult PhysicianProfile()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            return View(adminFunction.EditPhysician(phyid));
        }
        [Authorization("2")]
        public IActionResult PhysicianSchedule()
        {
            return View(adminFunction.Scheduling());
        }
        [Authorization("2")]
        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            return PartialView("AdminLayout/_MonthWise", physicianFunction.LoadSchedulingPartial(date, regionid, phyid));
        }
        [Authorization("2")]
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
        public IActionResult InvoicingPhy()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ProviderFinalizeTimeSheetModal modal = new ProviderFinalizeTimeSheetModal();
            modal.DropDate = firstDayOfMonth;
            return View(physicianFunction.FinalizeTimesheetPhy(modal, phyid));
        }
        public IActionResult FinalizeTimesheetPhy(ProviderFinalizeTimeSheetModal modal)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            return View(physicianFunction.FinalizeTimesheetPhy(modal, phyid));
        }
        public IActionResult ProviderFinalizeTimeSheetSubmit(ProviderFinalizeTimeSheetModal modal)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            string phyname = HttpContext.Session.GetString("physicianname")!;
            physicianFunction.ProviderFinalizeTimeSheetSubmit(modal, phyid, phyname);
            TempData["success"] = "Timesheet Submit Successfully";
            return RedirectToAction("InvoicingPhy");
        }
        [HttpPost]
        public bool ReceiptsSubmit(ReceiptsData modal)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            string phyname = HttpContext.Session.GetString("physicianname")!;
            return physicianFunction.ReceiptsSubmit(modal, phyid, phyname);
        }
        public void ReceiptsDelete(DateTime date)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            physicianFunction.ReceiptsDelete(date, phyid);
        }
        public IActionResult FinalizeTimeSheetTable(DateTime date)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            ProviderFinalizeTimeSheetModal modal = new ProviderFinalizeTimeSheetModal();
            modal.DropDate = date;
            return PartialView("PhysicianLayout/_FinalizedTimesheet", physicianFunction.FinalizeTimesheetPhy(modal, phyid));
        }
        public IActionResult FinalizeTimesheet(int id)
        {
            bool f = physicianFunction.FinalizeTimesheet(id);
            if (f == true)
            {
                TempData["success"] = "Timesheet Finalized Successfully";
            }
            else
            {
                TempData["error"] = "First Submit The Timesheet";
            }
            return RedirectToAction("InvoicingPhy");
        }
        public IActionResult FinalizeReimbursementTable(DateTime date)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid")!;
            ProviderFinalizeTimeSheetModal modal = new ProviderFinalizeTimeSheetModal();
            modal.DropDate = date;
            return PartialView("PhysicianLayout/_FinalizedReimbursement", physicianFunction.FinalizeTimesheetPhy(modal, phyid));
        }
        public IActionResult AdminsBox()
        {
            var jwtservice = HttpContext!.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice!.ValidateToken(token!, out JwtSecurityToken jwttoken);
            var aspnetUserId = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId")!.Value;
            return PartialView("PhysicianLayout/_AdminsBox", physicianFunction.AdminsBox(int.Parse(aspnetUserId)));
        }
    }
}
