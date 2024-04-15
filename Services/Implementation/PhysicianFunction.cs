using Data.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Data.DataModels;

namespace Services.Implementationy
{
    public class PhysicianFunction : IPhysicianFunction
    {
        private readonly ApplicationDbContext _context;

        public PhysicianFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public NewStateData AdminDashboarddata(int status, int currentPage, int phyid, string searchkey = "")
        {
            NewStateData data = new NewStateData();
            List<Data.DataModels.Request> req = new List<Data.DataModels.Request>();
            if (status == 1)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null).ToList();
            }
            else if (status == 2)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else if (status == 4)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => (u.Status == 4 || u.Status == 5) && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 6 && u.Physicianid == phyid).ToList();
            }
            data.newcount = getNewRequestCount(phyid);
            data.activecount = getActiveRequestCount(phyid);
            data.pendingcount = getPendingRequestCount(phyid);
            data.concludecount = getConcludeRequestCount(phyid);
            var regions = _context.Regions.ToList();
            data.regions = regions;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            data.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            var casetag = _context.Casetags.ToList();
            data.req = req;
            data.casetags = casetag;
            data.currentpage = currentPage;
            data.searchkey = searchkey;
            data.status = Convert.ToInt32(status);
            data.encounters = _context.Encounters.ToList();
            return data;
        }

        public int getActiveRequestCount(int phyid)
        {
            return _context.Requests.Count(rc => (rc.Status == 4 || rc.Status == 5) && rc.Physicianid == phyid && rc.Accepteddate != null);
        }

        public int getConcludeRequestCount(int phyid)
        {
            return _context.Requests.Count(u => u.Status == 6 && u.Physicianid == phyid);

        }

        public int getNewRequestCount(int phyid)
        {
            return _context.Requests.Count(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null);
        }

        public int getPendingRequestCount(int phyid)
        {
            return _context.Requests.Count(rc => rc.Status == 2 && rc.Physicianid == phyid && rc.Accepteddate != null);
        }
        public NewStateData AdminDashboard(int phyid)
        {
            NewStateData modal = new NewStateData();
            modal.concludecount = getConcludeRequestCount(phyid);
            modal.newcount = getNewRequestCount(phyid);
            modal.pendingcount = getPendingRequestCount(phyid);
            modal.activecount = getActiveRequestCount(phyid);
            return modal;
        }
        public NewStateData toogletable(string reqtypeid, string status, int currentPage, int phyid, string searchkey = "")
        {
            NewStateData newStateData = new NewStateData();
            List<Data.DataModels.Request> req = new List<Data.DataModels.Request>();
            if (status == "1")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null).ToList();
            }
            else if (status == "2")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 2 && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else if (status == "4")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && (u.Status == 4 || u.Status == 5) && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 6 && u.Physicianid == phyid).ToList();
            }
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;
            newStateData.reqtype = reqtypeid;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            newStateData.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            newStateData.currentpage = currentPage;
            newStateData.req = req;
            newStateData.searchkey = searchkey;
            newStateData.status = Convert.ToInt32(status);
            newStateData.encounters = _context.Encounters.ToList();
            return newStateData;
        }
        public void PhysicianAccept(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            if (req != null)
            {
                req.Accepteddate = DateTime.Now;
                _context.Requests.Update(req);
                _context.SaveChanges();
            }
        }
        public IActionResult DownloadEncounter(int id)
        {
            var request = _context.Requests.Include(u => u.Requestclients).FirstOrDefault(U => U.Requestid == id);
            var encounter = _context.Encounters.FirstOrDefault(x => x.RequestId == request.Requestid);
            EncounterFormViewModel model = new EncounterFormViewModel();
            if (request != null)
            {
                model.RequestId = request.Requestid;
                model.Firstname = request.Requestclients.First().Firstname;
                model.Lastname = request.Requestclients.First().Lastname;
            }
            model.DOB = new DateTime((int)request.Requestclients.First().Intyear, Convert.ToInt32(request.Requestclients.First().Strmonth), (int)request.Requestclients.First().Intdate).Date;
            model.Mobile = request.Requestclients.FirstOrDefault().Phonenumber;
            model.Email = request.Requestclients.FirstOrDefault().Email;
            model.Location = request.Requestclients.FirstOrDefault().Address;

            if (encounter != null)
            {
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
                model.isFinaled = encounter.IsFinalized[0];
            }
            var pdf = new iTextSharp.text.Document();
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdf, memoryStream);
                pdf.Open();

                // Add content to the PDF here. For example:
                pdf.Add(new Paragraph($"First Name: {model.Firstname}"));
                pdf.Add(new Paragraph($"Last Name: {model.Lastname}"));
                pdf.Add(new Paragraph($"DOB: {model.DOB}"));
                pdf.Add(new Paragraph($"Mobile: {model.Mobile}"));
                pdf.Add(new Paragraph($"Email: {model.Email}"));
                pdf.Add(new Paragraph($"Location: {model.Location}"));
                pdf.Add(new Paragraph($"History Of Illness: {model.HistoryOfIllness}"));
                pdf.Add(new Paragraph($"Medical History: {model.MedicalHistory}"));
                pdf.Add(new Paragraph($"Medication: {model.Medication}"));
                pdf.Add(new Paragraph($"Allergies: {model.Allergies}"));
                pdf.Add(new Paragraph($"Temp: {model.Temp}"));
                pdf.Add(new Paragraph($"HR: {model.HR}"));
                pdf.Add(new Paragraph($"RR: {model.RR}"));
                pdf.Add(new Paragraph($"BPs: {model.BPs}"));
                pdf.Add(new Paragraph($"BPd: {model.BPd}"));
                pdf.Add(new Paragraph($"O2: {model.O2}"));
                pdf.Add(new Paragraph($"Pain: {model.Pain}"));
                pdf.Add(new Paragraph($"Heent: {model.Heent}"));
                pdf.Add(new Paragraph($"CV: {model.CV}"));
                pdf.Add(new Paragraph($"Chest: {model.Chest}"));
                pdf.Add(new Paragraph($"ABD: {model.ABD}"));
                pdf.Add(new Paragraph($"Extr: {model.Extr}"));
                pdf.Add(new Paragraph($"Skin: {model.Skin}"));
                pdf.Add(new Paragraph($"Neuro: {model.Neuro}"));
                pdf.Add(new Paragraph($"Other: {model.Other}"));
                pdf.Add(new Paragraph($"Diagnosis: {model.Diagnosis}"));
                pdf.Add(new Paragraph($"Treatment Plan: {model.TreatmentPlan}"));
                pdf.Add(new Paragraph($"Medications Dispended: {model.MedicationsDispended}"));
                pdf.Add(new Paragraph($"Procedure: {model.Procedure}"));
                pdf.Add(new Paragraph($"Followup: {model.Followup}"));
                pdf.Add(new Paragraph($"Is Finaled: {model.isFinaled}"));

                pdf.Close();
                writer.Close();

                var bytes = memoryStream.ToArray();
                var result = new FileContentResult(bytes, "application/pdf");
                result.FileDownloadName = "Encounter_" + model.RequestId + ".pdf";
                return result;
            }
        }
        public void PhysicianNotesSaveChanges(int reqid, string physiciannotes, string physicianname)
        {
            var reqnotes = _context.Requestnotes.FirstOrDefault(u => u.Requestid == reqid);
            if (reqnotes == null)
            {
                Requestnote requestnote = new Requestnote
                {
                    Requestid = reqid,
                    Physiciannotes = physiciannotes,
                    Createdby = physicianname,
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(requestnote);
                _context.SaveChanges();
            }
            else
            {
                reqnotes.Physiciannotes = physiciannotes;
                _context.Requestnotes.Update(reqnotes);
                _context.SaveChanges();
            }
        }
    }
}
