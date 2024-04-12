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
        private readonly ApplicationDbContext _context;
        public PhysicianController(ApplicationDbContext context, IPhysicianFunction physicianFunction)
        {
            _context = context;
            this.physicianFunction = physicianFunction;
        }
        public IActionResult PhysicianDashboard()
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            if (HttpContext.Session.GetString("physicianname") != null)
            {
                return View(physicianFunction.AdminDashboard(phyid));
            }
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult ActiveStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(4, currentPage, phyid, searchkey));
        }
        public IActionResult NewStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(1, currentPage, phyid, searchkey));
        }
        public IActionResult PendingStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            if (reqtypeid != null && status != null)
            {
                return View(physicianFunction.toogletable(reqtypeid, status, currentPage, phyid, searchkey));
            }
            return View(physicianFunction.AdminDashboarddata(2, currentPage, phyid, searchkey));
        }
        public IActionResult ConcludeStatePhy(string reqtypeid, string status, int currentPage = 1, string searchkey = "")
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
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
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
            req.Status = 1;
            req.Physicianid = null;
            _context.Requests.Update(req);
            _context.SaveChanges();
            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = reqid,
                Status = 1,
                Physicianid = phyid,
                Notes = Assignnotes,
                Createddate = DateTime.Now,
                Transtoadmin = new BitArray(new[] { true })
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
            return RedirectToAction("PhysicianDashboard");
        }

        public IActionResult EncounterBtn(NewStateData modal, string[] encounterchk)
        {
            int phyid = (int)HttpContext.Session.GetInt32("physicianid");
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == modal.reqid);
            if (encounterchk[0] == "housecall")
            {
                req.Status = 5;
            }
            else
            {
                req.Status = 6;
                req.Calltype = 2;
            }
            _context.Requests.Update(req);
            _context.SaveChanges();
            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = req.Requestid,
                Status = req.Status,
                Physicianid = phyid,
                Notes = "Encounter Form",
                Createddate = DateTime.Now,
                Transtoadmin = new BitArray(new[] { false })
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult HouseCallBtn(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            req.Status = 6;
            req.Calltype = 1;
            _context.Requests.Update(req);
            _context.SaveChanges();
            return RedirectToAction("PhysicianDashboard");
        }
        public IActionResult EncounterForm(int id)
        {
            EncounterFormViewModel model = new EncounterFormViewModel();
            var reqclient = _context.Requestclients.Include(u => u.Request).FirstOrDefault(u => u.Requestid == id);
            model.Firstname = reqclient?.Firstname;
            model.Lastname = reqclient?.Lastname;
            if (reqclient?.Intyear != null && reqclient.Strmonth != null && reqclient.Intdate != null)
            {
                model.DOB = new DateTime((int)reqclient.Intyear, Convert.ToInt32(reqclient.Strmonth), (int)reqclient.Intdate);
            }
            model.Dateofservice = reqclient?.Request.Accepteddate;
            model.Mobile = reqclient?.Phonenumber;
            model.Email = reqclient?.Email;
            model.RequestId = id;
            model.Location = reqclient?.Location;
            var encounter = _context.Encounters.FirstOrDefault(u => u.RequestId == id);
            if (encounter != null)
            {
                model.Dateofservice = encounter.Date;
                model.HistoryOfIllness = encounter.HistoryIllness;
                model.MedicalHistory = encounter.MedicalHistory;
                model.Medication = encounter.Medications;
                model.Allergies = encounter.Allergies;
                model.Temp = encounter.Temp;
                model.HR = encounter.Hr;
                model.RR = encounter.Rr;
                model.BPs = encounter.BpS;
                model.BPd = encounter.BpD;
                model.O2 = encounter.O2;
                model.Pain = encounter.Pain;
                model.Heent = encounter.Heent;
                model.CV = encounter.Cv;
                model.Chest = encounter.Chest;
                model.ABD = encounter.Abd;
                model.Extr = encounter.Extr;
                model.Skin = encounter.Skin;
                model.Neuro = encounter.Neuro;
                model.Other = encounter.Other;
                model.Diagnosis = encounter.Diagnosis;
                model.TreatmentPlan = encounter.TreatmentPlan;
                model.MedicationsDispended = encounter.MedicationDispensed;
                model.Procedure = encounter.Procedures;
                model.Followup = encounter.FollowUp;
            }
            return View(model);
        }
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == model.RequestId);
            if (reqclient != null)
            {
                reqclient.Firstname = model.Firstname;
                reqclient.Lastname = model.Lastname;
                reqclient.Location = model.Location;
                reqclient.Intdate = model.DOB.Value.Day;
                reqclient.Strmonth = model.DOB.Value.Month.ToString();
                reqclient.Intyear = model.DOB.Value.Year;
                reqclient.Phonenumber = model.Mobile;
                reqclient.Email = model.Email;
            }
            _context.Requestclients.Update(reqclient);
            _context.SaveChanges();
            var check = false;
            var encounter = _context.Encounters.FirstOrDefault(u => u.RequestId == model.RequestId);
            if (encounter == null)
            {
                encounter = new Encounter();
                check = true;
            }
            encounter.RequestId = model.RequestId;
            encounter.Date = model.Dateofservice;
            encounter.HistoryIllness = model.HistoryOfIllness;
            encounter.MedicalHistory = model.MedicalHistory;
            encounter.Medications = model.Medication;
            encounter.Allergies = model.Allergies;
            encounter.Temp = model.Temp;
            encounter.Hr = model.HR;
            encounter.Rr = model.RR;
            encounter.BpS = model.BPs;
            encounter.BpD = model.BPd;
            encounter.O2 = model.O2;
            encounter.Pain = model.Pain;
            encounter.Heent = model.Heent;
            encounter.Cv = model.CV;
            encounter.Chest = model.Chest;
            encounter.Abd = model.ABD;
            encounter.Extr = model.Extr;
            encounter.Skin = model.Skin;
            encounter.Neuro = model.Neuro;
            encounter.Other = model.Other;
            encounter.Diagnosis = model.Diagnosis;
            encounter.TreatmentPlan = model.TreatmentPlan;
            encounter.MedicationDispensed = model.MedicationsDispended;
            encounter.Procedures = model.Procedure;
            encounter.FollowUp = model.Followup;
            encounter.IsFinalized = new BitArray(new[] { false });
            if (check == true)
            {
                _context.Encounters.Add(encounter);
            }
            else
            {
                _context.Encounters.Update(encounter);
            }
            _context.SaveChanges();
            TempData["success"] = "Data Saved Successfuly";
            return RedirectToAction("EncounterForm", new { id = model.RequestId });
        }
    }
}
