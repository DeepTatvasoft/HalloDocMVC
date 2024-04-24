using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;
using Authorization = Services.Implementation.Authorization;
using DataAccess.ServiceRepository.IServiceRepository;
using Data.DataContext;
using Newtonsoft.Json;
using Common.Helper;

namespace HalloDoc.Controllers
{

    public class AdminController : Controller
    {
        private readonly IAdminFunction adminFunction;
        private readonly IDashboard dashboard;
        private readonly IJwtRepository jwtRepository;
        private readonly IPhysicianFunction physicianFunction;
        public AdminController(IAdminFunction adminFunction, IDashboard dashboard, IJwtRepository jwtRepository, IPhysicianFunction physicianFunction)
        {
            this.adminFunction = adminFunction;
            this.dashboard = dashboard;
            this.jwtRepository = jwtRepository;
            this.physicianFunction = physicianFunction;
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
        [Authorization("1,2")]
        public IActionResult ViewCase(int id)
        {
            return PartialView("AdminLayout/_ViewCase", adminFunction.ViewCase(id));
        }
        [Authorization("1,2")]
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
                model.role = adminFunction.getrole(id);
                Response.Cookies.Append("jwt", jwtRepository.GenerateJwtToken(model));
                if (model.role == "1")
                {
                    var admin = adminFunction.GetAdmin(id);
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
                    var physician = adminFunction.GetPhysician(id);
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
        [Authorization("1")]
        public List<Physician> filterregion(string regionid)
        {
            return adminFunction.filterregion(regionid);
        }
        public IActionResult adminlogout()
        {
            if (HttpContext.Session.GetString("Adminname") != null)
            {
                HttpContext.Session.Remove("Adminname");
                TempData["success"] = "Admin Logged Out Successfuly";
            }
            else
            {
                HttpContext.Session.Remove("physicianname");
                TempData["success"] = "Physician Logged Out Successfuly";
            }
            Response.Cookies.Delete("jwt");
            return RedirectToAction("adminlogin", "Admin");
        }
        [Authorization("1")]
        public IActionResult cancelcase(int reqid, int casetagid, string cancelnotes)
        {
            int id = (int)HttpContext.Session.GetInt32("Adminid")!;
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.cancelcase(reqid, casetagid, cancelnotes, adminname, id);
            return RedirectToAction("AdminDashboard");
        }
        [Authorization("1")]
        public IActionResult assigncase(int reqid, int regid, int phyid, string Assignnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            int id = (int)HttpContext.Session.GetInt32("Adminid")!;
            adminFunction.assigncase(reqid, regid, phyid, Assignnotes, adminname, id);
            return RedirectToAction("AdminDashboard");
        }
        [Authorization("1")]
        public IActionResult blockcase(int reqid, string Blocknotes)
        {
            adminFunction.blockcase(reqid, Blocknotes);
            return RedirectToAction("AdminDashboard");
        }
        [Authorization("1")]
        [HttpPost]
        public IActionResult AdminNotesSaveChanges(int reqid, string adminnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.AdminNotesSaveChanges(reqid, adminnotes, adminname);
            return PartialView("AdminLayout/_ViewNotes", adminFunction.ViewNotes(reqid));
        }
        [Authorization("1,2")]
        public IActionResult AdminuploadDoc(int reqid)
        {
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }
        [Authorization("1,2")]
        [HttpPost]
        public IActionResult DocUpload(List<IFormFile> myfile, int reqid)
        {
            if (myfile.Count() != 0)
            {
                dashboard.AddPatientRequestWiseFile(myfile, reqid);
            }
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }
        [Authorization("1,2")]
        public IActionResult SingleDelete(int reqfileid)
        {
            int reqid = adminFunction.SingleDelete(reqfileid);
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }
        [Authorization("1,2")]
        public IActionResult DeleteAll(List<int> reqwiseid, int reqid)
        {
            foreach (var obj in reqwiseid)
            {
                adminFunction.SingleDelete(obj);
            }
            return PartialView("AdminLayout/_ViewDocument", adminFunction.AdminuploadDoc(reqid));
        }
        [Authorization("1,2")]
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
        [Authorization("1,2")]
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
        [Authorization("1")]
        [HttpPost]
        public IActionResult OrderSubmit(SendOrders sendorder)
        {
            adminFunction.OrderSubmit(sendorder);
            return RedirectToAction("AdminDashboard");
        }
        [Authorization("1")]
        public IActionResult clearcase(int reqid)
        {
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult ReviewAgreement(string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            return View(adminFunction.ReviewAgreement(id2));
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
        [Authorization("1")]
        public IActionResult CloseCase(int reqid)
        {
            return PartialView("AdminLayout/_CloseCase", adminFunction.AdminuploadDoc(reqid));
        }
        [Authorization("1")]
        public IActionResult CloseCasebtn(int id)
        {
            adminFunction.CloseCasebtn(id);
            return RedirectToAction("AdminDashboard");
        }
        [Authorization("1")]
        [HttpPost]
        public IActionResult Closecaseedit([FromForm] AdminviewDoc formData)
        {
            adminFunction.Closecaseedit(formData);
            return PartialView("AdminLayout/_CloseCase", adminFunction.AdminuploadDoc(formData.reqid));
        }

        [Authorization("1,2")]
        public IActionResult Profiletab()
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid")!;
            return View(adminFunction.Profiletab(adminid));
        }
        [HttpPost]
        public IActionResult AdminResetPassword(AdminProfile modal, string useaccess)
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
            if (useaccess == "1")
            {
                string aspid = adminFunction.getAsdId(modal.adminid);
                return RedirectToAction("EditAdmin", new { id = EncryptDecryptHelper.Encrypt(aspid) });
            }
            return RedirectToAction("Profiletab", "Admin");
        }
        [Authorization("1")]
        public IActionResult AdministratorinfoEdit(AdminProfile Modal, string useaccess)
        {
            var chk = Request.Form["AdminRegion"].ToList();
            var checkemail = adminFunction.AdministratorinfoEdit(Modal, chk!);
            if (checkemail == false)
            {
                TempData["error"] = "Email already exist";
            }
            if (useaccess == "1")
            {
                string aspid = adminFunction.getAsdId(Modal.adminid);
                return RedirectToAction("EditAdmin", new { id = EncryptDecryptHelper.Encrypt(aspid) });
            }
            return RedirectToAction("Profiletab", "Admin");
        }
        [Authorization("1")]
        public IActionResult MailinginfoEdit(AdminProfile modal, string useaccess)
        {
            adminFunction.MailinginfoEdit(modal);
            if (useaccess == "1")
            {
                string aspid = adminFunction.getAsdId(modal.adminid);
                return RedirectToAction("EditAdmin", new { id = EncryptDecryptHelper.Encrypt(aspid) });
            }
            return RedirectToAction("Profiletab", "Admin");
        }
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
        public IActionResult ProvidertabbyRegion(int regionid)
        {
            return PartialView("AdminLayout/_ProviderTable", adminFunction.Providertab(regionid));
        }
        [Authorization("1,2")]
        public IActionResult AdminCreateReq()
        {
            return View();
        }
        [Authorization("1")]
        public IActionResult EditPhysician(string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            return View(adminFunction.EditPhysician(id2));
        }
        [Authorization("1")]
        [HttpPost]
        public IActionResult PhysicianAccInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.PhysicianAccInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(modal.physicianid.ToString()) });
        }
        [Authorization("1")]
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
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(modal.physicianid.ToString()) });
        }
        [Authorization("1")]
        public IActionResult PhysicianInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;

            var chk = Request.Form["PhysicianRegion"].ToList();
            var checkemail = adminFunction.PhysicianInfo(modal, adminname, chk!);
            if (checkemail == false)
            {
                TempData["error"] = "Email already exist";
            }
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(modal.physicianid.ToString()) });
        }
        [Authorization("1")]
        public IActionResult PhysicianMailingInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.PhysicianMailingInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(modal.physicianid.ToString()) });
        }
        [Authorization("1")]
        public IActionResult ProviderProfile(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.ProviderProfile(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(modal.physicianid.ToString()) });
        }
        [Authorization("1")]
        public IActionResult EditProviderSign(int physicianid, string base64string)
        {
            adminFunction.EditProviderSign(physicianid, base64string);
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(physicianid.ToString()) });
        }
        [Authorization("1")]
        public IActionResult EditProviderPhoto(int physicianid, string base64string)
        {
            adminFunction.EditProviderPhoto(physicianid, base64string);
            return RedirectToAction("EditPhysician", new { id = EncryptDecryptHelper.Encrypt(physicianid.ToString()) });
        }
        [Authorization("1")]
        [HttpPost]
        public IActionResult PhyNotification()
        {
            var chk = Request.Form["chknotification"].ToList();
            adminFunction.PhyNotification(chk!);
            return RedirectToAction("Providertab");
        }
        [Authorization("1")]
        public IActionResult DeletePhysician(EditPhysicianModal modal)
        {
            adminFunction.DeletePhysician(modal);
            return RedirectToAction("Providertab");
        }
        [Authorization("1")]
        public IActionResult ContactPhysician(int phyid, string chk, string message)
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid")!;
            adminFunction.ContactPhysician(phyid, chk, message, adminid);
            return NoContent();
        }
        [Authorization("1")]
        public IActionResult AccessTab()
        {
            return View(adminFunction.AccessTab());
        }
        [Authorization("1")]
        public IActionResult CreateRole()
        {
            return PartialView("AdminLayout/_CreateRole", adminFunction.CreateRole());
        }
        [Authorization("1")]
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
        [Authorization("1")]
        public IActionResult EditRole(int roleid)
        {
            return PartialView("AdminLayout/_EditRole", adminFunction.EditRole(roleid));
        }
        [Authorization("1")]
        public IActionResult EditRoleSubmit(AccessRoleModal modal)
        {
            var chk = Request.Form["AllMenu"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname")!;
            if (chk.Count == 0 || modal.RoleName == null)
            {
                TempData["error"] = "Values Can't be Empty";
                return RedirectToAction("AccessTab");
            }
            adminFunction.EditRoleSubmit(modal, chk!, adminname);
            return RedirectToAction("AccessTab");
        }
        [Authorization("1")]
        public IActionResult DeleteRole(int roleid)
        {
            adminFunction.DeleteRole(roleid);
            return RedirectToAction("AccessTab");
        }
        [Authorization("1")]
        public IActionResult CreateProviderAcc()
        {
            return View(adminFunction.CreateProviderAcc());
        }
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1,2")]
        public IActionResult OpenFile(string fileName, int physicianid)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianDocuments", physicianid.ToString(), fileName);
            string mimeType = "application/pdf";
            Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;
            return PhysicalFile(filePath, mimeType);
        }
        public bool phyemailcheck(string email)
        {
            return adminFunction.phyemailcheck(email);
        }
        [Authorization("1,2")]
        public IActionResult Scheduling()
        {
            return View(adminFunction.Scheduling());
        }

        [Authorization("1,2")]
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
        [Authorization("1,2")]
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

        [Authorization("1,2")]
        public Scheduling viewshift(int shiftdetailid)
        {
            return adminFunction.viewshift(shiftdetailid);
        }
        [Authorization("1,2")]
        public void ViewShiftreturn(int shiftdetailid)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.ViewShiftreturn(shiftdetailid, adminname);
        }
        [Authorization("1,2")]
        public bool ViewShiftedit(Scheduling modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            return adminFunction.ViewShiftedit(modal, adminname);
        }
        [Authorization("1,2")]
        public void DeleteShift(int shiftdetailid)
        {
            string adminname = HttpContext.Session.GetString("Adminname")!;
            adminFunction.DeleteShift(shiftdetailid, adminname);
        }
        [Authorization("1")]
        public IActionResult ProvidersOnCall(Scheduling modal)
        {
            return View(adminFunction.ProvidersOnCall(modal));
        }
        [Authorization("1")]
        [HttpPost]
        public IActionResult ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall)
        {
            return PartialView("AdminLayout/_ProviderOnCallData", adminFunction.ProvidersOnCallbyRegion(regionid, oncall, offcall));
        }

        [Authorization("1")]
        public IActionResult ShiftForReview()
        {
            return View(adminFunction.ShiftForReview());
        }
        [Authorization("1")]
        public IActionResult ShiftReviewTable(int currentPage, int regionid)
        {
            return PartialView("AdminLayout/_ShiftForReviewTable", adminFunction.ShiftReviewTable(currentPage, regionid));
        }
        [Authorization("1")]
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
        [Authorization("1")]
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
        [Authorization("1")]
        public IActionResult PartnersTab()
        {
            return View(adminFunction.PartnersTab());
        }
        [Authorization("1")]
        public IActionResult PartenersbyType(int profftype)
        {
            return PartialView("AdminLayout/_PartnerstabTable", adminFunction.PartenersbyType(profftype));
        }
        [Authorization("1")]
        public IActionResult AddBusiness()
        {
            return View(adminFunction.AddBusiness());
        }
        [Authorization("1")]
        public IActionResult AddBusinessSubmit(AddBusinessModal modal)
        {
            adminFunction.AddBusinessSubmit(modal);
            TempData["success"] = "Business Added Successfuly";
            return RedirectToAction("PartnersTab");
        }
        [Authorization("1")]
        public IActionResult EditBusiness(string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            return View(adminFunction.EditBusiness(id2));
        }
        [Authorization("1")]
        public IActionResult EditBusinessSubmit(AddBusinessModal modal)
        {
            adminFunction.EditBusinessSubmit(modal);
            TempData["success"] = "Information updated successfuly";
            return RedirectToAction("PartnersTab");
        }
        [Authorization("1")]
        public void DeleteBusiness(int id)
        {
            adminFunction.DeleteBusiness(id);
            TempData["success"] = "Business Deleted Successfuly";
        }
        [Authorization("1")]
        public IActionResult Recordstab()
        {
            return View(adminFunction.Recordstab());
        }
        [Authorization("1")]
        public IActionResult RecordsTable(RecordstabModal modal)
        {
            return PartialView("AdminLayout/_RecordstabTable", adminFunction.RecordsTable(modal));
        }
        [Authorization("1")]
        public IActionResult BlockHistory()
        {
            return View(adminFunction.BlockHistory());
        }
        [Authorization("1")]
        public IActionResult BlockHistoryTable(BlockHistoryModal modal)
        {
            return PartialView("AdminLayout/_BlockHistoryTable", adminFunction.BlockHistoryTable(modal));
        }
        [Authorization("1")]
        public IActionResult EmailLogs()
        {
            return View(adminFunction.EmailLogs());
        }

        [Authorization("1")]
        public IActionResult EmailLogTable(EmailLogsModal modal)
        {
            return PartialView("AdminLayout/_EMaillogsTable", adminFunction.EmailLogTable(modal));
        }
        [Authorization("1")]
        public IActionResult SearchRecords()
        {
            return View(adminFunction.SearchRecords());
        }
        [Authorization("1")]
        public IActionResult SearchRecordTable(SearchRecordModal modal)
        {
            return PartialView("AdminLayout/_SearchrecordTable", adminFunction.SearchRecordTable(modal));
        }
        [Authorization("1")]
        public IActionResult ExplorePatient(string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            return View(adminFunction.ExplorePatient(id2));
        }
        [Authorization("1")]
        public IActionResult ExportSearchRecord(SearchRecordModal modal)
        {
            var record = adminFunction.ExportSearchRecord(adminFunction.ExportSearchRecordData(modal));
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{modal.reqstatus}_{strDate}.xlsx";
            return File(record, contentType, filename);
        }
        [Authorization("1")]
        public void DeleteSearchRecord(int id)
        {
            adminFunction.DeleteSearchRecord(id);
            TempData["success"] = "Request Deleted Successfuly";
        }
        [Authorization("1")]
        public IActionResult LocationTab()
        {
            LocationtabModal modal = new LocationtabModal();
            modal.physicianlocations = JsonConvert.SerializeObject(adminFunction.getPhyLocation());
            return View(modal);
        }
        [Authorization("1")]
        public IActionResult UserAccess()
        {
            return View(adminFunction.UserAccess());
        }
        [Authorization("1")]
        public IActionResult UserAccessTable(string roleid)
        {
            if (roleid == "0")
            {
                return PartialView("AdminLayout/_UserAccessTable", adminFunction.UserAccess());
            }
            return PartialView("AdminLayout/_UserAccessTable", adminFunction.UserAccessTable(roleid));
        }
        [Authorization("1")]
        public IActionResult editUser(int id)
        {
            return PartialView("_PatientProfile", dashboard.PatientDashboard(id));
        }
        [Authorization("1")]
        public IActionResult editUserByAdmin(PatientDashboardedit dashedit)
        {
            HttpContext.Session.SetString("Username", dashboard.editUser(dashedit, dashedit.userid));
            return RedirectToAction("UserAccess", adminFunction.UserAccess());
        }
        [Authorization("1")]
        public IActionResult EditAdmin(string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            int adminid = adminFunction.getAdminId(id2);
            return View(adminFunction.Profiletab(adminid));
        }
        [Authorization("1")]
        public IActionResult EncounterBtn(NewStateData modal, string[] encounterchk)
        {
            int adminid = (int)HttpContext.Session.GetInt32("Adminid")!;
            adminFunction.EncounterBtn(modal, encounterchk, adminid);
            return RedirectToAction("PhysicianDashboard", "Physician");
        }
        [Authorization("1")]
        public IActionResult EncounterFormAdmin(int id)
        {
            return View(physicianFunction.EncounterForm(id));
        }
        [Authorization("1")]
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            physicianFunction.EncounterFormSubmit(model);
            TempData["success"] = "Data Saved Successfuly";
            return RedirectToAction("EncounterFormAdmin", new { id = model.RequestId });
        }
        public void UnblockReq(int id)
        {
            adminFunction.UnblockReq(id);
        }
        public IActionResult AdminResetPass(string id)
        {
            id = id.Substring(3);
            id = EncryptDecryptHelper.Decrypt(id);
            ResetPasswordVM vm = new ResetPasswordVM();
            vm.Email = id;
            return View(vm);
        }
        public IActionResult AdminForgetPass()
        {
            return View();
        }
        public void RequestDTY(string message)
        {
            adminFunction.RequestDTY(message);
        }

    }

}
