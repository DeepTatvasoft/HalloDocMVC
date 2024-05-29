using Data.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Data.DataModels;
using Microsoft.AspNetCore.Http;
using System.Collections;
using Services.Implementation;

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
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 1 && u.Physicianid == phyid).ToList();
            }
            else if (status == 2)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 2 && u.Physicianid == phyid).ToList();
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
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname!.ToLower().Contains(searchkey.ToLower()))).ToList();
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
            return _context.Requests.Count(u => u.Status == 1 && u.Physicianid == phyid);
        }

        public int getPendingRequestCount(int phyid)
        {
            return _context.Requests.Count(rc => rc.Status == 2 && rc.Physicianid == phyid);
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
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 1 && u.Physicianid == phyid).ToList();
            }
            else if (status == "2")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 2 && u.Physicianid == phyid).ToList();
            }
            else if (status == "4")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && (u.Status == 4 || u.Status == 5) && u.Physicianid == phyid).ToList();
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
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname!.ToLower().Contains(searchkey.ToLower()))).ToList();
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
                req.Status = 2;
                _context.Requests.Update(req);
                _context.SaveChanges();
                Requeststatuslog requeststatuslog = new Requeststatuslog
                {
                    Requestid = req.Requestid,
                    Status = 2,
                    Physicianid = req.Physicianid,
                    Createddate = DateTime.Now,
                };
                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }
        }
        public IActionResult DownloadEncounter(int id)
        {
            var request = _context.Requests.Include(u => u.Requestclients).FirstOrDefault(U => U.Requestid == id);
            var encounter = _context.Encounters.FirstOrDefault(x => x.RequestId == request!.Requestid);
            EncounterFormViewModel model = new EncounterFormViewModel();
            if (request != null)
            {
                model.RequestId = request.Requestid;
                model.Firstname = request.Requestclients.First().Firstname;
                model.Lastname = request.Requestclients.First().Lastname;
            }
            model.DOB = new DateTime((int)request!.Requestclients.First().Intyear!, Convert.ToInt32(request.Requestclients.First().Strmonth), (int)request.Requestclients.First().Intdate!).Date;
            model.Mobile = request.Requestclients.FirstOrDefault()!.Phonenumber;
            model.Email = request.Requestclients.FirstOrDefault()!.Email;
            model.Location = request.Requestclients.FirstOrDefault()!.Address;

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
                model.isFinaled = encounter.IsFinalized![0];
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
        public void Transfer(int reqid, string Assignnotes, int phyid)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
            req!.Status = 1;
            req.Physicianid = null;
            req.Modifieddate = DateTime.Now;
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
        }
        public void EncounterBtn(NewStateData modal, string[] encounterchk, int phyid)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == modal.reqid);
            if (encounterchk[0] == "housecall")
            {
                req!.Status = 5;
                req.Calltype = 1;
            }
            else
            {
                req!.Status = 6;
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
        }
        public void HouseCallBtn(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            req!.Status = 6;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        public EncounterFormViewModel EncounterForm(int id)
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
            return model;
        }

        public void EncounterFormSubmit(EncounterFormViewModel model)
        {
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == model.RequestId);
            if (reqclient != null)
            {
                reqclient.Firstname = model.Firstname!;
                reqclient.Lastname = model.Lastname;
                reqclient.Location = model.Location;
                reqclient.Intdate = model.DOB!.Value.Day;
                reqclient.Strmonth = model.DOB.Value.Month.ToString();
                reqclient.Intyear = model.DOB.Value.Year;
                reqclient.Phonenumber = model.Mobile;
                reqclient.Email = model.Email;
            }
            _context.Requestclients.Update(reqclient!);
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
        }
        public void EncounterFormFinalize(EncounterFormViewModel modal)
        {
            var encounter = _context.Encounters.FirstOrDefault(u => u.RequestId == modal.RequestId);
            if (encounter != null)
            {
                encounter.IsFinalized = new BitArray(new[] { true });
                _context.Encounters.Update(encounter);
                _context.SaveChanges();
            }
        }
        public bool ConcludeCareBtn(AdminviewDoc modal, int phyid)
        {
            var enc = _context.Encounters.FirstOrDefault(u => u.RequestId == modal.reqid);
            if (enc == null || enc.IsFinalized![0] == false)
            {
                return false;
            }
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == modal.reqid);
            req!.Status = 8;
            req.Completedbyphysician = new BitArray(new[] { true });
            req.Modifieddate = DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = req.Requestid,
                Status = 8,
                Notes = modal.providernotes,
                Createddate = DateTime.Now,
                Transtoadmin = new BitArray(new[] { false }),
                Physicianid = phyid
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
            return true;
        }
        public MonthWiseScheduling LoadSchedulingPartial(string date, int regionid, int phyid)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _context.Physicians.ToList();
            MonthWiseScheduling month = new MonthWiseScheduling
            {
                date = currentDate,
                shiftdetails = _context.Shiftdetailregions.Include(u => u.Shiftdetail).ThenInclude(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Shiftdetail.Shift.Physicianid == phyid && u.Isdeleted == new BitArray(new[] { false })).Where(u => u.Regionid == regionid).Select(u => u.Shiftdetail).ToList()
            };
            if (regionid == 0)
            {
                month.shiftdetails = _context.Shiftdetails.Include(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Shift.Physicianid == phyid && u.Isdeleted == new BitArray(new[] { false })).ToList();
            }
            return month;
        }

        public ProviderFinalizeTimeSheetModal FinalizeTimesheetPhy(ProviderFinalizeTimeSheetModal modal, int phyid)
        {
            var firstDayOfMonth = new DateTime(modal.DropDate.Year, modal.DropDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var startDate = modal.DropDate;
            DateTime endDate;
            if (modal.DropDate.Day == 1)
            {
                endDate = startDate.AddDays(13);
            }
            else
            {
                endDate = lastDayOfMonth;
            }
            var timesheet = _context.Timesheets.Where(u => u.Physicianid == phyid && u.Date >= startDate && u.Date <= endDate).OrderBy(u => u.Date);
            modal.isfinalized = false;
            modal.isapproved = false;
            if (timesheet.Count() > 0)
            {
                var biweek = _context.Biweektimes.FirstOrDefault(u => u.Biweekid == timesheet.First().Biweektimeid);
                modal.biweektime = biweek!;
                modal.isfinalized = (bool)biweek!.Isfinalized!;
                modal.isapproved = (bool)biweek.Isapproved!;
                modal.biweekid = biweek.Biweekid;
                modal.Rows = new List<TimeSheetData>();
                foreach (var obj in timesheet)
                {
                    TimeSheetData timesheetData = new TimeSheetData();
                    timesheetData.OnCallHours = (int?)(obj.Oncallhours ?? 0);
                    timesheetData.IsHoliday = (bool)obj.Isweekend!;
                    timesheetData.NumberOfHouseCalls = obj.Housecall ?? 0;
                    timesheetData.NumberOfPhoneConsults = obj.Consult ?? 0;
                    timesheetData.TotalHours = timesheetData.OnCallHours;
                    modal.Rows.Add(timesheetData);
                }
            }
            var reimbursement = _context.Reimbursements.Where(u => u.Physicianid == phyid && u.Date >= startDate && u.Date <= endDate && u.Isdeleted == false);
            if (reimbursement.Count() > 0)
            {
                modal.receiptsDatas = new List<ReceiptsData>();
                foreach (var obj in reimbursement)
                {
                    ReceiptsData receiptsData = new ReceiptsData();
                    receiptsData.Date = (DateTime)obj.Date!;
                    receiptsData.itemname = obj.Item;
                    receiptsData.amount = (int?)obj.Amount;
                    receiptsData.filename = obj.Bill;
                    modal.receiptsDatas.Add(receiptsData);
                }

            }
            modal.physicians = _context.Physicians.ToList();
            modal.physicianid = phyid;
            modal.phyname = modal.physicians.FirstOrDefault(u => u.Physicianid == phyid)!.Firstname;
            return modal;
        }
        public void ProviderFinalizeTimeSheetSubmit(ProviderFinalizeTimeSheetModal modal, int phyid, string phyname)
        {
            var bikweektime = _context.Biweektimes.FirstOrDefault(u => u.Physicianid == phyid && u.Firstday == modal.Rows[0].Date);
            if (bikweektime == null)
            {
                Biweektime biweektime = new Biweektime
                {
                    Physicianid = phyid,
                    Isfinalized = false,
                    Firstday = modal.Rows[0].Date,
                    Lastday = modal.Rows[modal.Rows.Count() - 1].Date,
                    Isapproved = false
                };
                _context.Biweektimes.Add(biweektime);
                _context.SaveChanges();
                foreach (var obj in modal.Rows)
                {
                    var timesheet = new Timesheet();
                    timesheet.Physicianid = phyid;
                    timesheet.Isweekend = obj.IsHoliday;
                    timesheet.Oncallhours = obj.OnCallHours??0;
                    timesheet.Housecall = obj.NumberOfHouseCalls??0;
                    timesheet.Consult = obj.NumberOfPhoneConsults??0;
                    timesheet.Createdby = phyname;
                    timesheet.Biweektimeid = biweektime.Biweekid;
                    timesheet.Date = obj.Date;
                    timesheet.Createddate = DateTime.Now;
                    _context.Timesheets.Add(timesheet);
                    _context.SaveChanges();
                }
            }
            else
            {
                foreach (var obj in modal.Rows)
                {
                    var timesheet = _context.Timesheets.FirstOrDefault(t => t.Physicianid == phyid && t.Date == obj.Date);
                    timesheet!.Physicianid = phyid;
                    timesheet.Isweekend = obj.IsHoliday;
                    timesheet.Oncallhours = obj.OnCallHours;
                    timesheet.Housecall = obj.NumberOfHouseCalls;
                    timesheet.Consult = obj.NumberOfPhoneConsults;
                    timesheet.Modifiedby = phyname;
                    timesheet.Date = obj.Date;
                    timesheet.Modifieddate = DateTime.Now;
                    _context.Timesheets.Update(timesheet);
                    _context.SaveChanges();
                }
            }
        }
        public void ReceiptsDoc(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReceiptsBill");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        public bool ReceiptsSubmit(ReceiptsData modal, int phyid, string phyname)
        {
            var biweek = _context.Biweektimes.FirstOrDefault(u => u.Physicianid == phyid && u.Firstday <= modal.Date && u.Lastday >= modal.Date);
            if (biweek == null)
            {
                return false;
            }
            var reimbursement = _context.Reimbursements.FirstOrDefault(u => u.Physicianid == phyid && u.Date == modal.Date && u.Isdeleted == false);
            var f = 0;
            if (reimbursement == null)
            {
                reimbursement = new Reimbursement();
                f = 1;
            }
            reimbursement.Physicianid = phyid;
            reimbursement.Item = modal.itemname;
            reimbursement.Amount = modal.amount;
            reimbursement.Isdeleted = false;
            reimbursement.Biweektimeid = biweek.Biweekid;
            reimbursement.Date = modal.Date;
            if (modal.bill != null)
            {
                reimbursement.Bill = modal.bill!.FileName;
                ReceiptsDoc(modal.bill!);
            }
            if (f == 1)
            {
                reimbursement.Createdby = phyname;
                reimbursement.Createddate = DateTime.Now;
                _context.Reimbursements.Add(reimbursement);
            }
            else
            {
                reimbursement.Modifieddate = DateTime.Now;
                reimbursement.Modifirdby = phyname;
                _context.Reimbursements.Update(reimbursement);
            }
            _context.SaveChanges();
            return true;
        }
        public void ReceiptsDelete(DateTime date, int phyid)
        {
            var reimbursement = _context.Reimbursements.FirstOrDefault(u => u.Physicianid == phyid && u.Date == date);
            reimbursement!.Isdeleted = true;
            _context.Reimbursements.Update(reimbursement);
            _context.SaveChanges();
        }
        public bool FinalizeTimesheet(int id)
        {
            var biweek = _context.Biweektimes.FirstOrDefault(u => u.Biweekid == id);
            if (biweek == null)
            {
                return false;
            }
            biweek!.Isfinalized = true;
            _context.Biweektimes.Update(biweek);
            _context.SaveChanges();
            return true;
        }
        public AdminsBoxModal AdminsBox(int aspnetUserId)
        {
            var model = new AdminsBoxModal
            {
                sId = aspnetUserId,
                Admins = _context.Admins.ToList()
            };
            return model;
        }
    }
}
