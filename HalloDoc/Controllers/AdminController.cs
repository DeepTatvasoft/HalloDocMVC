using Data.DataModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Authorization = Services.Implementation.Authorization;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Data.DataContext;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using Syncfusion.EJ2.Spreadsheet;
using Org.BouncyCastle.Ocsp;
using static NPOI.HSSF.Util.HSSFColor;
using Syncfusion.EJ2.Charts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HalloDoc.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminFunction adminFunction;
        private readonly IDashboard dashboard;
        private readonly IJwtRepository jwtRepository;
        public AdminController(ApplicationDbContext context, IAdminFunction adminFunction, IDashboard dashboard, IJwtRepository jwtRepository)
        {
            _context = context;
            this.adminFunction = adminFunction;
            this.dashboard = dashboard;
            this.jwtRepository = jwtRepository;
        }
        public IActionResult adminlogin()
        {
            return View();
        }
        [Authorization("1")]
        public IActionResult ActiveState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(4, currentPage, searchkey));
        }
        [Authorization("1")]
        public IActionResult NewState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(1, currentPage, searchkey));
        }
        [Authorization("1")]
        public IActionResult PendingState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(2, currentPage, searchkey));
        }
        [Authorization("1")]
        public IActionResult TocloseState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(3, currentPage, searchkey));
        }
        [Authorization("1")]
        public IActionResult UnpaidState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(9, currentPage, searchkey));
        }
        [Authorization("1")]
        public IActionResult ConcludeState(string reqtypeid, string status, int regionid, int currentPage = 1, string searchkey = "")
        {
            if (regionid != 0 && reqtypeid != null)
            {
                return View(adminFunction.RegionReqtype(regionid, reqtypeid, status, currentPage, searchkey));
            }
            if (regionid != 0 && status != null && reqtypeid == null)
            {
                return View(adminFunction.regiontable(regionid, status, currentPage, searchkey));
            }
            if (reqtypeid != null && status != null)
            {
                return View(adminFunction.toogletable(reqtypeid, status, currentPage, searchkey));
            }
            return View(adminFunction.AdminDashboarddata(6, currentPage, searchkey));
        }
        public IActionResult ViewCase(int id)
        {
            return PartialView("AdminLayout/_ViewCase", adminFunction.ViewCase(id));
        }
        public IActionResult ViewNotes(int reqid)
        {
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }
        [Authorization("1")]
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("Adminname") != null)
            {
                return View(adminFunction.AdminDashboard());
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
                LoggedInPersonViewModel model = new LoggedInPersonViewModel();
                model.aspuserid = id;
                model.username = adminname;
                model.role = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == id.ToString())!.Roleid;
                //model.userid = _context.Users.FirstOrDefault(u => u.Aspnetuserid == id).Userid;
                Response.Cookies.Append("jwt", jwtRepository.GenerateJwtToken(model));
                if (model.role == "1")
                {
                    var admin = _context.Admins.FirstOrDefault(x => x.Aspnetuserid == id.ToString());
                    HttpContext.Session.SetString("Adminname", adminname);
                    HttpContext.Session.SetInt32("Adminid", admin!.Adminid);
                    if (HttpContext.Session.GetString("Adminname") != null)
                    {
                        TempData["success"] = "Admin LogIn Successfully";
                    }
                    return RedirectToAction("Admindashboard", "Admin");
                }
                else
                {
                    var physician = _context.Physicians.FirstOrDefault(u => u.Aspnetuserid == id.ToString());
                    HttpContext.Session.SetString("physicianname", adminname);
                    HttpContext.Session.SetInt32("physicianid", physician!.Physicianid);
                    if (HttpContext.Session.GetString("physicianname") != null)
                    {
                        TempData["success"] = "Physician LogIn Successfully";
                    }
                    return RedirectToAction("PhysicianDashboard", "Physician");
                }
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
            Response.Cookies.Delete("jwt");
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult cancelcase(int reqid, int casetagid, string cancelnotes)
        {
            int id = (int)HttpContext.Session.GetInt32("Adminid")!;
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.cancelcase(reqid, casetagid, cancelnotes, adminname, id);
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult assigncase(int reqid, int regid, int phyid, string Assignnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            int id = (int)HttpContext.Session.GetInt32("Adminid")!;
            adminFunction.assigncase(reqid, regid, phyid, Assignnotes, adminname, id);
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult blockcase(int reqid, string Blocknotes)
        {
            adminFunction.blockcase(reqid, Blocknotes);
            return RedirectToAction("AdminDashboard");
        }
        [HttpPost]
        public IActionResult AdminNotesSaveChanges(int reqid, string adminnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.AdminNotesSaveChanges(reqid, adminnotes, adminname);
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }

        public IActionResult AdminuploadDoc(int reqid)
        {
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }
        [HttpPost]
        public IActionResult DocUpload(List<IFormFile> myfile, int reqid)
        {
            if (myfile.Count() != 0)
            {
                dashboard.AddPatientRequestWiseFile(myfile, reqid);
            }
            _context.SaveChanges();
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }

        public IActionResult SingleDelete(int reqfileid)
        {
            int reqid = adminFunction.SingleDelete(reqfileid);
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }

        public IActionResult DeleteAll(List<int> reqwiseid, int reqid)
        {
            foreach (var obj in reqwiseid)
            {
                adminFunction.SingleDelete(obj);
            }
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }

        public IActionResult SendMail(List<int> reqwiseid, int reqid)
        {

            List<string> filenames = adminFunction.SendMail(reqwiseid, reqid);
            Sendemail("yashb.patel@etatvasoft.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));

        }
        public async Task Sendemail(string email, string subject, string message, List<string> attachmentPaths)
        {
            try
            {
                var mail = "tatva.dotnet.deeppatel@outlook.com";
                var password = "Deep2292002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);

                foreach (var attachmentPath in attachmentPaths)
                {
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        public IActionResult Orders(int reqid)
        {
            SendOrders modal = new SendOrders();
            modal.reqid = reqid;
            modal.createdby = HttpContext.Session.GetString("Adminname");
            return PartialView("AdminLayout/_SendOrder", modal);
        }
        public List<Healthprofessionaltype> getprofession()
        {
            return adminFunction.getprofession();
        }
        public List<Healthprofessional> filterprofession(int professionid)
        {
            return adminFunction.filterprofession(professionid);
        }
        public Healthprofessional filterbusiness(int vendorid)
        {
            return adminFunction.filterbusiness(vendorid);
        }
        [HttpPost]
        public IActionResult OrderSubmit(SendOrders sendorder)
        {
            adminFunction.OrderSubmit(sendorder);
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult clearcase(int reqid)
        {
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult ReviewAgreement(int id)
        {
            return View(adminFunction.ReviewAgreement(id));
        }
        public IActionResult AcceptAgreement(int id)
        {
            adminFunction.AcceptAgreement(id);
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult CancelAgreement(Agreementmodal modal)
        {
            adminFunction.CancelAgreement(modal);
            return RedirectToAction("patientlogin", "Home");
        }
        public IActionResult CloseCase(int reqid)
        {
            return PartialView("AdminLayout/_CloseCase", adminFunction.AdminuploadDoc(reqid));
        }
        public IActionResult CloseCasebtn(int id)
        {
            adminFunction.CloseCasebtn(id);
            return RedirectToAction("AdminDashboard");
        }
        [HttpPost]
        public IActionResult Closecaseedit([FromForm] AdminviewDoc formData)
        {
            adminFunction.Closecaseedit(formData);
            return PartialView("AdminLayout/_CloseCase", adminFunction.AdminuploadDoc(formData.reqid));
        }

        [Authorization("1")]
        public IActionResult Profiletab()
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid")!;
            return View(adminFunction.Profiletab(adminid));
        }
        [HttpPost]
        public IActionResult AdminResetPassword(AdminProfile modal)
        {
            if (modal.ResetPassword == null)
            {
                TempData["error"] = "Please enter valid password";
            }
            else
            {
                TempData["success"] = "Your Password is changed Successfuly";
                adminFunction.AdminResetPassword(modal);
            }
            return RedirectToAction("Profiletab", "Admin");
        }
        public IActionResult AdministratorinfoEdit(AdminProfile Modal)
        {
            var chk = Request.Form["AdminRegion"].ToList();
            var checkemail = adminFunction.AdministratorinfoEdit(Modal, chk!);
            if (checkemail == false)
            {
                TempData["error"] = "Email already exist";
            }
            return RedirectToAction("Profiletab", "Admin");
        }
        public IActionResult MailinginfoEdit(AdminProfile modal)
        {
            adminFunction.MailinginfoEdit(modal);
            return RedirectToAction("Profiletab", "Admin");
        }
        [HttpPost]
        public IActionResult Export(NewStateData modal)
        {
            var record = new byte[0];
            NewStateData data = new NewStateData();
            if (modal.region != 0 && modal.reqtype != null)
            {
                record = adminFunction.DownloadExcle(adminFunction.RegionReqtype(modal.region, modal.reqtype, modal.status.ToString(), 0, modal.searchkey));
            }
            else if (modal.region != 0 && modal.status.ToString() != null && modal.reqtype == null)
            {
                record = adminFunction.DownloadExcle(adminFunction.regiontable(modal.region, modal.status.ToString(), 0, modal.searchkey));
            }
            else if (modal.reqtype != null && modal?.status != null)
            {
                record = adminFunction.DownloadExcle(adminFunction.toogletable(modal.reqtype, modal.status.ToString(), 0, modal.searchkey));
            }
            else
            {
                record = adminFunction.DownloadExcle(adminFunction.AdminDashboarddata(1, 0, modal!.searchkey));
            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{modal.region}_{strDate}.xlsx";
            return File(record, contentType, filename);
        }
        public IActionResult ExportAll(NewStateData modal)
        {
            var record = adminFunction.DownloadExcle(adminFunction.AdminDashboarddata(modal.status, 0, ""));
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{modal.region}_{strDate}.xlsx";
            return File(record, contentType, filename);
        }
        [Authorization("1")]
        public IActionResult Providertab()
        {
            return View(adminFunction.Providertab(0));
        }
        public IActionResult ProvidertabbyRegion(int regionid)
        {
            return PartialView("AdminLayout/_ProviderTable", adminFunction.Providertab(regionid));
        }
        [Authorization("1")]
        public IActionResult AdminCreateReq()
        {
            return View();
        }
        [Authorization("1")]
        public IActionResult EditPhysician(int id)
        {
            return View(adminFunction.EditPhysician(id));
        }
        [HttpPost]
        public IActionResult PhysicianAccInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.PhysicianAccInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physicianid });
        }
        public IActionResult PhysicianResetPass(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (modal.password != null)
            {
                adminFunction.PhysicianResetPass(modal, adminname);
                TempData["success"] = "Your Password is changed Successfuly";
            }
            else
            {
                TempData["error"] = "Please enter valid password";
            }
            return RedirectToAction("EditPhysician", new { id = modal.physicianid });
        }
        public IActionResult PhysicianInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;

            var chk = Request.Form["PhysicianRegion"].ToList();
            var checkemail = adminFunction.PhysicianInfo(modal, adminname, chk!);
            if (checkemail == false)
            {
                TempData["error"] = "Email already exist";
            }
            return RedirectToAction("EditPhysician", new { id = modal.physicianid });
        }
        public IActionResult PhysicianMailingInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.PhysicianMailingInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physicianid });
        }
        public IActionResult ProviderProfile(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.ProviderProfile(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physicianid });
        }
        public IActionResult EditProviderSign(int physicianid, string base64string)
        {
            adminFunction.EditProviderSign(physicianid, base64string);
            return RedirectToAction("EditPhysician", new { id = physicianid });
        }
        public IActionResult EditProviderPhoto(int physicianid, string base64string)
        {
            adminFunction.EditProviderPhoto(physicianid, base64string);
            return RedirectToAction("EditPhysician", new { id = physicianid });
        }
        [HttpPost]
        public IActionResult PhyNotification()
        {
            var chk = Request.Form["chknotification"].ToList();
            adminFunction.PhyNotification(chk!);
            return RedirectToAction("Providertab");
        }
        public IActionResult DeletePhysician(EditPhysicianModal modal)
        {
            adminFunction.DeletePhysician(modal);
            return RedirectToAction("Providertab");
        }
        public IActionResult ContactPhysician(int phyid, string chk, string message)
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid")!;
            adminFunction.ContactPhysician(phyid, chk, message, adminid);
            return NoContent();
        }
        public IActionResult AccessTab()
        {
            return View(adminFunction.AccessTab());
        }
        public IActionResult CreateRole()
        {
            return PartialView("AdminLayout/_CreateRole", adminFunction.CreateRole());
        }
        public IActionResult CreateRoleSubmit(AccessRoleModal modal)
        {
            var chk = Request.Form["AllMenu"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (chk.Count == 0 || modal.accountType == 3 || modal.RoleName == null)
            {
                TempData["error"] = "Values Can't be Empty";
                return RedirectToAction("AccessTab");
            }
            adminFunction.CreateRoleSubmit(modal, chk!, adminname);
            return RedirectToAction("AccessTab");
        }
        public List<Menu> filtermenu(int acctype)
        {
            return adminFunction.filtermenu(acctype);
        }
        public IActionResult EditRole(int roleid)
        {
            return PartialView("AdminLayout/_EditRole", adminFunction.EditRole(roleid));
        }
        public IActionResult EditRoleSubmit(AccessRoleModal modal)
        {
            var chk = Request.Form["AllMenu"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (chk.Count == 0 || modal.accountType == 3 || modal.RoleName == null)
            {
                TempData["error"] = "Values Can't be Empty";
                return RedirectToAction("AccessTab");
            }
            adminFunction.EditRoleSubmit(modal, chk!, adminname);
            return RedirectToAction("AccessTab");
        }
        public IActionResult DeleteRole(int roleid)
        {
            adminFunction.DeleteRole(roleid);
            return RedirectToAction("AccessTab");
        }
        public IActionResult CreateProviderAcc()
        {
            return View(adminFunction.CreateProviderAcc());
        }
        [HttpPost]
        public IActionResult CreateProviderAccBtn(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            var chk = Request.Form["PhysicianRegion"].ToList();
            adminFunction.CreateProviderAccBtn(modal, chk!, adminname);
            return RedirectToAction("Providertab");
        }
        public IActionResult CreateAdminAcc()
        {
            return View(adminFunction.CreateAdminAcc());
        }
        public IActionResult CreateAdminAccBtn(AdminProfile modal)
        {
            var chk = Request.Form["AdminRegion"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.CreateAdminAccBtn(modal, chk!, adminname);
            return RedirectToAction("AccessTab");
        }
        public void AddPhysicianDoc(IFormFile file, int physicianid, string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianDocuments", physicianid.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, filename);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        [HttpPost]
        public bool UploadICAdoc(int physicianid, IFormFile file)
        {
            string filetype = file.ContentType;
            if (filetype != "application/pdf")
            {
                TempData["error"] = "Upload document in pdf format";
                return false;

            }
            string filename = "ICA.pdf";
            AddPhysicianDoc(file, physicianid, filename);
            adminFunction.phyuploadDoc(physicianid, "ICA");
            TempData["success"] = "Document uploaded successfuly";
            return true;
        }
        [HttpPost]
        public bool UploadHIPAAdoc(int physicianid, IFormFile file)
        {
            string filetype = file.ContentType;
            if (filetype != "application/pdf")
            {
                TempData["error"] = "Upload document in pdf format";
                return false;

            }
            string filename = "HIPAA.pdf";
            AddPhysicianDoc(file, physicianid, filename);
            adminFunction.phyuploadDoc(physicianid, "HIPAA");
            TempData["success"] = "Document uploaded successfuly";
            return true;
        }
        [HttpPost]
        public bool UploadBGcheckdoc(int physicianid, IFormFile file)
        {
            string filetype = file.ContentType;
            if (filetype != "application/pdf")
            {
                TempData["error"] = "Upload document in pdf format";
                return false;

            }
            string filename = "BGCheck.pdf";
            AddPhysicianDoc(file, physicianid, filename);
            adminFunction.phyuploadDoc(physicianid, "BGCheck");
            TempData["success"] = "Document uploaded successfuly";
            return true;
        }
        [HttpPost]
        public bool UploadNDdoc(int physicianid, IFormFile file)
        {
            string filetype = file.ContentType;
            if (filetype != "application/pdf")
            {
                TempData["error"] = "Upload document in pdf format";
                return false;

            }
            string filename = "NDDoc.pdf";
            AddPhysicianDoc(file, physicianid, filename);
            adminFunction.phyuploadDoc(physicianid, "NDDoc");
            TempData["success"] = "Document uploaded successfuly";
            return true;
        }
        [HttpPost]
        public bool UploadLDdoc(int physicianid, IFormFile file)
        {
            string filetype = file.ContentType;
            if (filetype != "application/pdf")
            {
                TempData["error"] = "Upload document in pdf format";
                return false;

            }
            string filename = "LDDoc.pdf";
            AddPhysicianDoc(file, physicianid, filename);
            adminFunction.phyuploadDoc(physicianid, "LDDoc");
            TempData["success"] = "Document uploaded successfuly";
            return true;
        }
        public IActionResult OpenFile(string fileName, int physicianid)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianDocuments", physicianid.ToString(), fileName);
            string mimeType = "application/pdf";
            Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;
            return PhysicalFile(filePath, mimeType);
        }
        public bool phyemailcheck(string email)
        {
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == email);
            if (aspnetuser != null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Scheduling()
        {
            return View(adminFunction.Scheduling());
        }

        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = adminFunction.GetPhysicians();

            switch (PartialName)
            {
                case "_DayWise":
                    return PartialView("AdminLayout/_DayWise", adminFunction.Daywise(regionid, currentDate, physician));

                case "_WeekWise":
                    return PartialView("AdminLayout/_WeekWise", adminFunction.Weekwise(regionid, currentDate, physician));

                case "_MonthWise":
                    return PartialView("AdminLayout/_MonthWise", adminFunction.Monthwise(regionid, currentDate, physician));

                default:
                    return PartialView("AdminLayout/_DayWise");
            }
        }
        public IActionResult AddShift(Scheduling model)
        {
            if (model.starttime > model.endtime)
            {
                TempData["error"] = "Starttime Must be Less than Endtime";
                return RedirectToAction("Scheduling");
            }
            string adminname = HttpContext.Session.GetString("Adminname")!;
            var chk = Request.Form["repeatdays"].ToList();
            bool f = adminFunction.AddShift(model, adminname, chk!);
            if (f == false)
            {
                TempData["error"] = "Shift is already assigned in this time";
            }
            return RedirectToAction("Scheduling");
        }


        public Scheduling viewshift(int shiftdetailid)
        {
            return adminFunction.viewshift(shiftdetailid);
        }
        public void ViewShiftreturn(int shiftdetailid)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.ViewShiftreturn(shiftdetailid, adminname);
        }
        public bool ViewShiftedit(Scheduling modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            return adminFunction.ViewShiftedit(modal, adminname);
        }
        public void DeleteShift(int shiftdetailid)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.DeleteShift(shiftdetailid, adminname);
        }
        public IActionResult ProvidersOnCall(Scheduling modal)
        {
            return View(adminFunction.ProvidersOnCall(modal));
        }
        [HttpPost]
        public IActionResult ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall)
        {
            return PartialView("AdminLayout/_ProviderOnCallData", adminFunction.ProvidersOnCallbyRegion(regionid, oncall, offcall));
        }

        public IActionResult ShiftForReview()
        {
            return View(adminFunction.ShiftForReview());
        }
        public IActionResult ShiftReviewTable(int currentPage, int regionid)
        {
            return PartialView("AdminLayout/_ShiftForReviewTable", adminFunction.ShiftReviewTable(currentPage, regionid));
        }
        public IActionResult ApproveSelected(int[] shiftchk)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                adminFunction.ApproveSelected(shiftchk, adminname);
                TempData["success"] = "Shifts Approved Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }
        public IActionResult DeleteSelected(int[] shiftchk)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                adminFunction.DeleteSelected(shiftchk, adminname);
                TempData["success"] = "Shifts Deleted Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }
        public IActionResult PartnersTab()
        {
            return View(adminFunction.PartnersTab());
        }
        public IActionResult PartenersbyType(int profftype)
        {
            PartnersModal modal = new PartnersModal();
            modal.healthprofessionals = _context.Healthprofessionals.Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            if (profftype != 0)
            {
                modal.healthprofessionals = _context.Healthprofessionals.Where(u => u.Profession == profftype && u.Isdeleted == new BitArray(new[] { false })).ToList();
            }
            modal.healthprofessionaltypes = _context.Healthprofessionaltypes.Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            return PartialView("AdminLayout/_PartnerstabTable", modal);
        }
        public IActionResult AddBusiness()
        {
            AddBusinessModal modal = new AddBusinessModal();
            modal.healthprofessionaltypes = _context.Healthprofessionaltypes.Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            return View(modal);
        }
        public IActionResult AddBusinessSubmit(AddBusinessModal modal)
        {
            adminFunction.AddBusinessSubmit(modal);
            TempData["success"] = "Business Added Successfuly";
            return RedirectToAction("PartnersTab");
        }
        public IActionResult EditBusiness(int id)
        {
            return View(adminFunction.EditBusiness(id));
        }
        public IActionResult EditBusinessSubmit(AddBusinessModal modal)
        {
            adminFunction.EditBusinessSubmit(modal);
            TempData["success"] = "Information updated successfuly";
            return RedirectToAction("PartnersTab");
        }
        public void DeleteBusiness(int id)
        {
            adminFunction.DeleteBusiness(id);
            TempData["success"] = "Business Deleted Successfuly";
        }
        public IActionResult Recordstab()
        {
            return View(adminFunction.Recordstab());
        }
        public IActionResult RecordsTable(RecordstabModal modal)
        {
            return PartialView("AdminLayout/_RecordstabTable", adminFunction.RecordsTable(modal));
        }
        public IActionResult BlockHistory()
        {
            return View(adminFunction.BlockHistory());
        }
        public IActionResult BlockHistoryTable(BlockHistoryModal modal)
        {
            return PartialView("AdminLayout/_BlockHistoryTable", adminFunction.BlockHistoryTable(modal));
        }
        public IActionResult EmailLogs()
        {
            return View(adminFunction.EmailLogs());
        }

        public IActionResult EmailLogTable(EmailLogsModal modal)
        {
            return PartialView("AdminLayout/_EMaillogsTable", adminFunction.EmailLogTable(modal));
        }
        public IActionResult SearchRecords()
        {
            return View(adminFunction.SearchRecords());
        }
        public IActionResult SearchRecordTable(SearchRecordModal modal)
        {
            return PartialView("AdminLayout/_SearchrecordTable", adminFunction.SearchRecordTable(modal));
        }
        public IActionResult ExplorePatient(int id)
        {
            return View(adminFunction.ExplorePatient(id));
        }
        public IActionResult ExportSearchRecord(SearchRecordModal modal)
        {
            var record = adminFunction.ExportSearchRecord(adminFunction.ExportSearchRecordData(modal));
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{modal.reqstatus}_{strDate}.xlsx";
            return File(record, contentType, filename);
        }
        public void DeleteSearchRecord(int id)
        {
            adminFunction.DeleteSearchRecord(id);
            TempData["success"] = "Request Deleted Successfuly";
        }
        public IActionResult LocationTab()
        {
            LocationtabModal modal = new LocationtabModal();
            modal.physicianlocations = JsonConvert.SerializeObject(_context.Physicianlocations.ToList());
            return View(modal);
        }
        public IActionResult UserAccess()
        {
            return View(adminFunction.UserAccess());
        }
        public IActionResult UserAccessTable(string roleid)
        {
            if (roleid == "0")
            {
                return PartialView("AdminLayout/_UserAccessTable", adminFunction.UserAccess());
            }
            UserAccessModal modal = new UserAccessModal();
            var aspnetusers = _context.Aspnetusers.ToList();
            List<Aspnetuser> newaspuser = new List<Aspnetuser>();
            foreach (var obj in aspnetusers)
            {
                if(_context.Aspnetuserroles.FirstOrDefault(u=>u.Userid == obj.Id.ToString())!.Roleid == roleid)
                {
                    newaspuser.Add(obj);
                }
            }
            modal.aspnetusers = newaspuser;
            modal.aspnetroles = _context.Aspnetroles.ToList();
            modal.aspnetuserroles = _context.Aspnetuserroles.ToList();
            modal.admincount = _context.Requests.ToList().Count();
            modal.req = _context.Requests.Include(u => u.Physician).Include(u=>u.User).ToList();
            return PartialView("AdminLayout/_UserAccessTable", modal);
        }
    }

}
