using HalloDoc.DataContext;
using HalloDoc.Models;
using HalloDoc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using System.IO.Compression;

namespace HalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int reqid)
        {
            foreach (var item in formFile)
            {
                string filename = reqid.ToString() + "_" + item.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(fileStream);
                }

                //Request? req = _context.Requests.FirstOrDefault(i => i.Requestid == reqid);
                //int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Createddate = DateTime.Now,
                    Requestid = reqid,
                    Filename = path
                };

                _context.Requestwisefiles.Add(data3);
            }
            _context.SaveChanges();
        }
        public IActionResult PatientDashboard()
        {
            //User user = new User();
            if (HttpContext.Session.GetInt32("Userid") != null)
            {
                int temp = (int)HttpContext.Session.GetInt32("Userid");
                var tempname = HttpContext.Session.GetString("Username");
                //IEnumerable<Request> data = _context.Requests.Where(u => u.Userid == temp);
                //return View(data);
                PatientDashboardedit dashedit = new PatientDashboardedit();
                var data = _context.Users.FirstOrDefault(u => u.Userid == temp);
                dashedit.User = data;
                DateTime tempDateTime = new DateTime(Convert.ToInt32(data.Intyear), Convert.ToInt32(data.Strmonth), (int)data.Intdate);
                dashedit.tempdate = tempDateTime;
                List<Requestwisefile> reqfile = (from m in _context.Requestwisefiles select m).ToList();
                dashedit.requestwisefiles = reqfile;
                var requestdata = _context.Requests.Where(u => u.Userid == temp);
                dashedit.requests = requestdata.ToList();
                return View(dashedit);
            }
            else
            {
                return RedirectToAction("patientlogin", "Home");
            }
        }
        public IActionResult editUser(PatientDashboardedit dashedit)
        {
            int id = (int)HttpContext.Session.GetInt32("Userid");
            int aspid = (int)HttpContext.Session.GetInt32("AspUserid");
            var user = _context.Users.FirstOrDefault(u => u.Userid == id);
            user.Firstname = dashedit.User.Firstname;
            user.Lastname = dashedit.User.Lastname;
            string str = user.Firstname + " " + user.Lastname;
            HttpContext.Session.SetString("Username", str);
            user.Intdate = dashedit.tempdate.Day;
            user.Strmonth = (dashedit.tempdate.Month).ToString();
            user.Intyear = dashedit.tempdate.Year;
            user.Email = dashedit.User.Email;
            user.Mobile = dashedit.User.Mobile;
            user.Street = dashedit.User.Street;
            user.City = dashedit.User.City;
            user.State = dashedit.User.State;
            user.Zipcode = dashedit.User.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();

            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Id == aspid);
            aspuser.Username = str;
            _context.Aspnetusers.Update(aspuser);
            _context.SaveChanges();

            var req = _context.Requests.FirstOrDefault(u => u.Userid == id);
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == req.Requestid);
            reqclient.Firstname = dashedit.User.Firstname;
            reqclient.Lastname = dashedit.User.Lastname;
            _context.Requestclients.Update(reqclient);
            _context.SaveChanges();
            return RedirectToAction("PatientDashboard", "Dashboard");
        }
        public IActionResult ViewDocument(int id)
        {
            int temp = id;
            int uid = (int)HttpContext.Session.GetInt32("Userid");
            var tempname = HttpContext.Session.GetString("Username");
            PatientDashboardedit dashedit = new PatientDashboardedit();
            var data = _context.Users.FirstOrDefault(u => u.Userid == uid);
            dashedit.User = data;
            List<Requestwisefile> reqfile = (from m in _context.Requestwisefiles where m.Requestid == temp select m).ToList();
            dashedit.requestwisefiles = reqfile;
            var requestdata = _context.Requests.Where(u => u.Requestid == temp);
            dashedit.requests = requestdata.ToList();
            return View(dashedit);
        }
        [HttpPost]
        public IActionResult DocUpload(PatientDashboardedit dashedit)
        {
            if (dashedit.Upload != null)
            {
                AddPatientRequestWiseFile(dashedit.Upload, dashedit.reqid);
            }
            _context.SaveChanges();
            return RedirectToAction("ViewDocument", "Dashboard", new { id = dashedit.reqid });
        }

        [HttpPost]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(PatientDashboardedit dashedit)
        {
            var chk = Request.Form["checklist"].ToList();
            if (chk.Count == 0)
            {
                return NoContent();
            }
            using (var memorystream = new MemoryStream())
                {
                    using (var zip = new ZipArchive(memorystream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in chk)
                        {
                            var s = Int32.Parse(item);
                            var file = await _context.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
                            var path = file.Filename;
                            var bytes = await System.IO.File.ReadAllBytesAsync(path);
                            var zipEntry = zip.CreateEntry(file.Filename.Split("\\document\\")[1], CompressionLevel.Fastest);
                            using (var zipStream = zipEntry.Open())
                            {
                                await zipStream.WriteAsync(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    memorystream.Position = 0; // Reset the position
                    return File(memorystream.ToArray(), "application/zip", "file.zip", enableRangeProcessing: true);
                }
        }


        [Route("SingleDownload")]
        public async Task<IActionResult> SingleDownload(int id)
        {
            var fname = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == id).Filename;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", fname);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }
    }
}
