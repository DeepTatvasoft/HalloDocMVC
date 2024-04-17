using Data.DataContext;
using Data.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class FormSubmit : IFormSubmit
    {
        private readonly ApplicationDbContext _context;
        public FormSubmit(ApplicationDbContext context)
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


        public void patientinfo(PatientReqSubmit model,int adminid)
        {

            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email)!;
            User user = _context.Users.FirstOrDefault(u => u.Email == model.Email)!;
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            if (aspuser == null && user == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Email = model.Email;
                string username = model.FirstName + model.LastName;
                aspnetuser1.Username = username;
                aspnetuser1.Phonenumber = model.PhoneNumber;
                aspnetuser1.Passwordhash = model.Password;
                aspnetuser1.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Add(aspnetuser1);
                aspuser = aspnetuser1;

                User user1 = new User
                {
                    Firstname = model.FirstName!,
                    Lastname = model.LastName,
                    Email = model.Email!,
                    Mobile = model.PhoneNumber,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Zipcode = model.Zipcode,
                    Createdby = model.FirstName + model.LastName,
                    Createddate = DateTime.Now,
                    Intdate = model.DOB.Day,
                    Intyear = model.DOB.Year,
                    Strmonth = model.DOB.Month.ToString(),
                    Aspnetuser = aspuser,
                    Regionid = 1
                };
                _context.Users.Add(user1);
                user = user1;
                Aspnetuserrole aspnetuserrole = new Aspnetuserrole
                {
                    Userid = user.Aspnetuserid.ToString()!,
                    Roleid = "3"
                };
                _context.Aspnetuserroles.Add(aspnetuserrole);
            }
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == 3);
            Request req = new Request
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                Createddate = DateTime.Now,
                Requesttypeid = 1,
                Status = 1,
                User = user,
                Isdeleted = new BitArray(new[] { false }),
                Confirmationnumber = (region!.Abbreviation!.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName!.Substring(0, 2) + model.FirstName!.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
            };

            _context.Requests.Add(req);

            Requestclient reqclient = new Requestclient
            {
                Request = req,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                Notes = model.Symptoms,
                State = model.State,
                City = model.City,
                Street = model.Street,
                Zipcode = model.Zipcode,
                Intdate = model.DOB.Day,
                Intyear = model.DOB.Year,
                Strmonth = model.DOB.Month.ToString(),
                Location = model.Room,
                Regionid = 2,
                Address = model.Room + model.Street + model.City + model.State,
            };
            _context.Requestclients.Add(reqclient);
            if (model.Upload != null)
            {
                AddPatientRequestWiseFile(model.Upload, req.Requestid);
            }
            if(model.AdminNotes != null)
            {
                Requestnote reqnotes = new Requestnote
                {
                    Request = req,
                    Adminnotes = model.AdminNotes,
                    Createdby = _context.Admins.FirstOrDefault(u => u.Adminid == adminid)!.Firstname,
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(reqnotes);
            }
            _context.SaveChanges();
        }

        public (bool, int) familyinfo(FamilyFriendReqSubmit model)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == model.PatEmail)!;
            if (user != null)
            {
                var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
                var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
                Request req = new Request
                {
                    Firstname = model.FamFirstName,
                    Lastname = model.FamLastName,
                    Phonenumber = model.FamMobile,
                    Email = model.FamEmail,
                    Requesttypeid = 2,
                    Relationname = model.FamRelation,
                    Createddate = DateTime.Now,
                    Status = 1,
                    Confirmationnumber = (region!.Abbreviation!.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.PatLastName!.Substring(0, 2) + model.PatFirstName!.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    Isdeleted = new BitArray(new[] { false }),
                    User = user,
                };

                _context.Requests.Add(req);
                _context.SaveChanges();
                Requestclient reqclient = new Requestclient
                {
                    Notes = model.PatSymptoms,
                    Firstname = model.PatFirstName,
                    Lastname = model.PatLastName,
                    Intdate = model.PatDOB.Day,
                    Intyear = model.PatDOB.Year,
                    Strmonth = model.PatDOB.Month.ToString(),
                    Phonenumber = model.PatPhoneNumber,
                    Street = model.PatStreet,
                    City = model.PatCity,
                    State = model.PatState,
                    Zipcode = model.PatZipcode,
                    Regionid = 1,
                    Location = model.PatRoom,
                    Email = model.PatEmail,
                    Address = model.PatRoom + model.PatStreet + model.PatCity + model.PatState,
                    Request = req
                };
                Aspnetuserrole aspnetuserrole = new Aspnetuserrole
                {
                    Userid = user.Aspnetuserid.ToString()!,
                    Roleid = "3"
                };
                _context.Aspnetuserroles.Add(aspnetuserrole);
                _context.Requestclients.Add(reqclient);
                _context.SaveChanges();
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, req.Requestid);
                }
                return (true, reqclient.Requestclientid);
            }
            else
            {
                Request req = new Request
                {
                    Firstname = model.FamFirstName,
                    Lastname = model.FamLastName,
                    Phonenumber = model.FamMobile,
                    Email = model.FamEmail,
                    Requesttypeid = 2,
                    Relationname = model.FamRelation,
                    Createddate = DateTime.Now,
                    Status = 1,
                    Isdeleted = new BitArray(new[] { false }),
                };
                _context.Requests.Add(req);
                _context.SaveChanges();
                Requestclient reqclient = new Requestclient
                {
                    Notes = model.PatSymptoms,
                    Firstname = model.PatFirstName!,
                    Lastname = model.PatLastName,
                    Intdate = model.PatDOB.Day,
                    Intyear = model.PatDOB.Year,
                    Strmonth = model.PatDOB.Month.ToString(),
                    Phonenumber = model.PatPhoneNumber,
                    Street = model.PatStreet,
                    City = model.PatCity,
                    State = model.PatState,
                    Zipcode = model.PatZipcode,
                    Location = model.PatRoom,
                    Email = model.PatEmail,
                    Regionid = 1,
                    Address = model.PatRoom + model.PatStreet + model.PatCity + model.PatState,
                    Request = req
                };
                _context.Requestclients.Add(reqclient);
                _context.SaveChanges();
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, req.Requestid);
                }
                return (false, reqclient.Requestclientid);
            }
        }

        public void ConciergeInfo(ConciergeSubmit model)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == model.PatEmail)!;
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            string name = model.ConFirstName + model.ConLastName;
            Concierge concierge = new Concierge
            {
                Conciergename = name,
                Address = model.ConProperty,
                Street = model.ConStreet!,
                City = model.ConCity!,
                State = model.ConState!,
                Zipcode = model.ConZipcode!,
                Createddate = DateTime.Now,
                Regionid = 1
            };
            _context.Concierges.Add(concierge);
            Request req = new Request
            {
                Firstname = model.ConFirstName,
                Lastname = model.ConLastName,
                Phonenumber = model.ConPhonenumber,
                Email = model.ConEmail,
                Createddate = DateTime.Now,
                Requesttypeid = 3,
                Confirmationnumber = (region!.Abbreviation!.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.PatLastName!.Substring(0, 2) + model.PatFirstName!.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                User = user,
                Isdeleted = new BitArray(new[] { false }),
            };
            _context.Requests.Add(req);
            _context.SaveChanges();

            Requestclient reqclient = new Requestclient
            {
                Notes = model.PatSymptoms,
                Firstname = model.PatFirstName,
                Lastname = model.PatLastName,
                Email = model.PatEmail,
                Intdate = model.PatDOB.Day,
                Intyear = model.PatDOB.Year,
                Strmonth = model.PatDOB.Month.ToString(),
                Phonenumber = model.PatPhoneNumber,
                Street = model.PatStreet,
                City = model.PatCity,
                State = model.PatState,
                Zipcode = model.PatZipcode,
                Location = model.PatRoom,
                Address = model.PatRoom + model.PatStreet + model.PatCity + model.PatState,
                Request = req
            };
            _context.Requestclients.Add(reqclient);
            Aspnetuserrole aspnetuserrole = new Aspnetuserrole
            {
                Userid = user.Aspnetuserid.ToString()!,
                Roleid = "3"
            };
            _context.Aspnetuserroles.Add(aspnetuserrole);
            _context.SaveChanges();
        }

        public void BusinessInfo(BusinessSubmit model)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == model.PatEmail)!;
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            Request req = new Request
            {
                Firstname = model.BusFirstname,
                Lastname = model.BusLastname,
                Phonenumber = model.BusPhonenumber,
                Email = model.BusEmail,
                Requesttypeid = 4,
                Status = 1,
                Createddate = DateTime.Now,
                Confirmationnumber = (region!.Abbreviation!.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.PatLastName!.Substring(0, 2) + model.PatFirstName!.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                Isdeleted = new BitArray(new[] { false }),
                User = user,
            };
            _context.Requests.Add(req);
            _context.SaveChanges();

            Requestclient reqclient = new Requestclient
            {
                Notes = model.PatSymptoms,
                Firstname = model.PatFirstName,
                Lastname = model.PatLastName,
                Email = model.PatEmail,
                Intdate = model.PatDOB.Day,
                Intyear = model.PatDOB.Year,
                Strmonth = model.PatDOB.Month.ToString(),
                Phonenumber = model.PatPhoneNumber,
                Street = model.PatStreet,
                City = model.PatCity,
                State = model.PatState,
                Zipcode = model.PatZipcode,
                Location = model.PatRoom,
                Address = model.PatRoom + model.PatStreet + model.PatCity + model.PatState,
                Request = req
            };
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();

            Business bus = new Business
            {
                Name = model.BusFirstname + model.BusLastname,
                City = model.PatCity,
                Regionid = 1,
                Createddate = DateTime.Now,
                Zipcode = model.PatZipcode,
                Phonenumber = model.PatPhoneNumber,
                Businesstypeid = 1
            };
            _context.Businesses.Add(bus);
            Aspnetuserrole aspnetuserrole = new Aspnetuserrole
            {
                Userid = user.Aspnetuserid.ToString()!,
                Roleid = "3"
            };
            _context.Aspnetuserroles.Add(aspnetuserrole);
            _context.SaveChanges();
        }
        public void Emailentry(string email,int adminid,int id)
        {
            Emaillog emaillog = new Emaillog
            {
                Subjectname = "hello",
                Emailid = email,
                Emailtemplate = "hello Create Account https://localhost:44325/Home/CreateAccount/id=" + id + "",
                Roleid = 3,
                Requestid = id,
                Adminid = adminid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(new[] { true }),
                Senttries = 1
            };
            _context.Emaillogs.Add(emaillog);
            _context.SaveChanges();
        }
    }
}
