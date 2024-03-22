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
                model.role = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == id.ToString()).Roleid;
                //model.userid = _context.Users.FirstOrDefault(u => u.Aspnetuserid == id).Userid;
                Response.Cookies.Append("jwt", jwtRepository.GenerateJwtToken(model));
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
            Response.Cookies.Delete("jwt");
            return RedirectToAction("adminlogin", "Admin");
        }
        public IActionResult cancelcase(int reqid, int casetagid, string cancelnotes)
        {
            int id = (int)HttpContext.Session.GetInt32("Adminid");
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.cancelcase(reqid, casetagid, cancelnotes, adminname, id);
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult assigncase(int reqid, int regid, int phyid, string Assignnotes)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            int id = (int)HttpContext.Session.GetInt32("Adminid");
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
            string adminname = HttpContext.Session.GetString("Adminname");
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
            int adminid = (int)HttpContext.Session.GetInt32("Adminid");
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
            adminFunction.AdministratorinfoEdit(Modal, chk);
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
            else if (modal.reqtype != null && modal.status != null)
            {
                record = adminFunction.DownloadExcle(adminFunction.toogletable(modal.reqtype, modal.status.ToString(), 0, modal.searchkey));
            }
            else
            {
                record = adminFunction.DownloadExcle(adminFunction.AdminDashboarddata(1, 0, modal.searchkey));
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
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.PhysicianAccInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physician.Physicianid });
        }
        public IActionResult PhysicianResetPass(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            if (modal.password != null)
            {
                adminFunction.PhysicianResetPass(modal, adminname);
                TempData["success"] = "Your Password is changed Successfuly";
            }
            else
            {
                TempData["error"] = "Please enter valid password";
            }
            return RedirectToAction("EditPhysician", new { id = modal.physician.Physicianid });
        }
        public IActionResult PhysicianInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname");

            var chk = Request.Form["PhysicianRegion"].ToList();
            adminFunction.PhysicianInfo(modal, adminname, chk);
            return RedirectToAction("EditPhysician", new { id = modal.physician.Physicianid });
        }
        public IActionResult PhysicianMailingInfo(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.PhysicianMailingInfo(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physician.Physicianid });
        }
        public IActionResult ProviderProfile(EditPhysicianModal modal)
        {
            string adminname = HttpContext.Session.GetString("Adminname");
            adminFunction.ProviderProfile(modal, adminname);
            return RedirectToAction("EditPhysician", new { id = modal.physician.Physicianid });
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
            adminFunction.PhyNotification(chk);
            return RedirectToAction("Providertab");
        }
        public IActionResult DeletePhysician(EditPhysicianModal modal)
        {
            adminFunction.DeletePhysician(modal);
            return RedirectToAction("Providertab");
        }
        public IActionResult ContactPhysician(int phyid, string chk, string message)
        {
            adminFunction.ContactPhysician(phyid, chk, message);
            return NoContent();
        }
        public IActionResult AccessTab()
        {
            AccessRoleModal modal = new AccessRoleModal();
            modal.roles = _context.Roles.Where(U=>U.Isdeleted == new BitArray(new[] { false })).ToList();
            return View(modal);
        }
        public IActionResult CreateRole()
        {
            AccessRoleModal modal = new AccessRoleModal();
            modal.menu = _context.Menus.ToList();
            return PartialView("AdminLayout/_CreateRole", modal);
        }
        public IActionResult CreateRoleSubmit(AccessRoleModal modal)
        {
            var chk = Request.Form["AllMenu"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname");
            if (chk.Count == 0 || modal.accountType == 3 || modal.RoleName == null)
            {
                TempData["error"] = "Values Can't be Empty";
                return RedirectToAction("AccessTab");
            }
            Role role = new Role
            {
                Name = modal.RoleName,
                Accounttype = (short)modal.accountType,
                Createddate = DateTime.Now,
                Createdby = adminname,
                Isdeleted = new BitArray(new[] { false })
            };
            _context.Roles.Add(role);
            _context.SaveChanges();
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Rolemenu rolemenu = new Rolemenu
                {
                    Roleid = role.Roleid,
                    Menuid = s
                };
                _context.Rolemenus.Add(rolemenu);
            }
            _context.SaveChanges();
            return RedirectToAction("AccessTab");
        }
        public List<Menu> filtermenu(int acctype)
        {
            List<Menu> menu = _context.Menus.Where(u => u.Accounttype == acctype).ToList();
            if (acctype == 3)
            {
                menu = _context.Menus.ToList();
            }
            return menu;
        }
        public IActionResult EditRole(int roleid)
        {
            AccessRoleModal modal = new AccessRoleModal();
            var role = _context.Roles.FirstOrDefault(u => u.Roleid == roleid);
            modal.menu = _context.Menus.Where(u => u.Accounttype == role.Accounttype).ToList();
            modal.selectedmenuid = _context.Rolemenus.Include(r => r.Role).Where(r => r.Roleid == roleid && r.Role.Accounttype == role.Accounttype).Select(r => r.Menu.Menuid).ToList();
            modal.RoleName = role.Name;
            modal.accountType = role.Accounttype;
            modal.roleid = roleid;
            return PartialView("AdminLayout/_EditRole", modal);
        }
        public IActionResult EditRoleSubmit(AccessRoleModal modal)
        {
            var chk = Request.Form["AllMenu"].ToList();
            string adminname = HttpContext.Session.GetString("Adminname");
            if (chk.Count == 0 || modal.accountType == 3 || modal.RoleName == null)
            {
                TempData["error"] = "Values Can't be Empty";
                return RedirectToAction("AccessTab");
            }
            Role role = _context.Roles.FirstOrDefault(u => u.Roleid == modal.roleid);

            role.Name = modal.RoleName;
            role.Accounttype = (short)modal.accountType;
            role.Modifieddate = DateTime.Now;
            role.Modifiedby = adminname;
            _context.Roles.Update(role);
            _context.SaveChanges();
            var rolemenu = _context.Rolemenus.Where(u => u.Roleid == modal.roleid).ToList();
            foreach(var obj  in rolemenu)
            {
                _context.Rolemenus.Remove(obj);
            }
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Rolemenu newrolemenu = new Rolemenu
                {
                    Roleid = role.Roleid,
                    Menuid = s
                };
                _context.Rolemenus.Add(newrolemenu);
            }
            _context.SaveChanges();
            return RedirectToAction("AccessTab");
        }
        public IActionResult DeleteRole(int roleid)
        {
            var role = _context.Roles.FirstOrDefault(u=>u.Roleid == roleid);
            role.Isdeleted = new BitArray(new[] { true });
            _context.Roles.Update(role);
            _context.SaveChanges();
            return RedirectToAction("AccessTab");
        }
    }
}
