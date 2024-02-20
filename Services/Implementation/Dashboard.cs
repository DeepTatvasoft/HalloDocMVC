using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class Dashboard : IDashboard
    {
        private readonly ApplicationDbContext _context;

        public Dashboard(ApplicationDbContext context)
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
        public PatientDashboardedit PatientDashboard(int temp, string tempname)
        {
            PatientDashboardedit dashedit = new PatientDashboardedit();
            var data = _context.Users.FirstOrDefault(u => u.Userid == temp);
            dashedit.User = data;
            DateTime tempDateTime = new DateTime(Convert.ToInt32(data.Intyear), Convert.ToInt32(data.Strmonth), (int)data.Intdate);
            dashedit.tempdate = tempDateTime;
            List<Requestwisefile> reqfile = (from m in _context.Requestwisefiles select m).ToList();
            dashedit.requestwisefiles = reqfile;
            var requestdata = _context.Requests.Where(u => u.Userid == temp);
            dashedit.requests = requestdata.ToList();
            return dashedit;
        }
        public string editUser(PatientDashboardedit dashedit, int id, int aspid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Userid == id);
            user.Firstname = dashedit.User.Firstname;
            user.Lastname = dashedit.User.Lastname;
            string str = user.Firstname + " " + user.Lastname;
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
            return str;
        }

        public PatientDashboardedit ViewDocument(int temp, int uid, string tempname)
        {
            PatientDashboardedit dashedit = new PatientDashboardedit();
            var data = _context.Users.FirstOrDefault(u => u.Userid == uid);
            dashedit.User = data;
            List<Requestwisefile> reqfile = (from m in _context.Requestwisefiles where m.Requestid == temp select m).ToList();
            dashedit.requestwisefiles = reqfile;
            var requestdata = _context.Requests.Where(u => u.Requestid == temp);
            dashedit.requests = requestdata.ToList();
            return dashedit;
        }
        public (byte[], string, string) FileDownload(int id)
        {
            var fname = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == id).Filename;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", fname);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(path);
            return (bytes, contentType, Path.GetFileName(path));
        }
        public (byte[],string,string) DownloadFile(PatientDashboardedit dashedit,List<string> chk)
        {
            using (var memorystream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memorystream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in chk)
                    {
                        var s = Int32.Parse(item);
                        var file = _context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == s);
                        var path = file.Filename;
                        var bytes = System.IO.File.ReadAllBytes(path);
                        var zipEntry = zip.CreateEntry(file.Filename.Split("\\document\\")[1], CompressionLevel.Fastest);
                        using (var zipStream = zipEntry.Open())
                        {
                            zipStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                memorystream.Position = 0; // Reset the position
                return (memorystream.ToArray(), "application/zip", "file.zip");
            }
        }
    }
}

