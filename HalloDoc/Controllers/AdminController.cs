using Data.DataModels;
using HalloDoc.DataContext;
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
        [Authorization("1")]
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
    }
}
