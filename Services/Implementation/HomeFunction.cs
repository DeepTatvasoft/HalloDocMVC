using Common.Helper;
using Data.DataContext;
using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class HomeFunction : IHomeFunction
    {
        private readonly ApplicationDbContext _context;
        public HomeFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public (Aspnetuser, User) ValidateUser([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj == null)
            {
                return (null!, null!);
            }
            var user = _context.Users.FirstOrDefault(u => u.Aspnetuserid == obj.Id);
            return (obj!, user!);
        }
        public (bool, string) changepassword(ResetPasswordVM vm, string id)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == id);
            if (vm.Password == vm.ConfirmPassword)
            {
                aspuser!.Passwordhash = vm.Password;
                _context.Aspnetusers.Update(aspuser);
                _context.SaveChanges();
                return (true!, vm.Email!);
            }
            else
            {
                return (false!, vm.Email!);
            }
        }
        public Aspnetuser getaspuser(PatientReqSubmit model)
        {
            return _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email)!;
        }

        public void newaccount(PatientReqSubmit model, string id)
        {
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            Aspnetuser aspnetuser = new Aspnetuser();
            var req = _context.Requests.Include(u => u.Requestclients).FirstOrDefault(u => u.Requestid == id2);
            aspnetuser.Email = model.Email;
            aspnetuser.Passwordhash = model.ConfirmPassword;
            aspnetuser.Username = req!.Firstname + " " + req.Lastname;
            aspnetuser.Phonenumber = req.Phonenumber;
            aspnetuser.Modifieddate = DateTime.Now;
            _context.Aspnetusers.Add(aspnetuser);
            User user = new User
            {
                Firstname = req.Firstname!,
                Lastname = req.Lastname,
                Email = model.Email!,
                Aspnetuser = aspnetuser,
                Createdby = req.Firstname!,
                Intdate = req.Requestclients.ElementAt(0).Intdate,
                Intyear = req.Requestclients.ElementAt(0).Intyear,
                Strmonth = req.Requestclients.ElementAt(0).Strmonth,
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            Aspnetuserrole aspnetuserrole = new Aspnetuserrole
            {
                Userid = user.Aspnetuserid.ToString()!,
                Roleid = "3"
            };
            _context.Aspnetuserroles.Add(aspnetuserrole);
            _context.SaveChanges();
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == req.Requestclients.ElementAt(0).Regionid);
            req!.User = user;
            req.Confirmationnumber = (region!.Abbreviation!.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + req.Firstname!.Substring(0, 2) + req.Lastname!.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper();
            _context.Requests.Update(req);
            _context.SaveChanges();
        }
    }
}
