using Common.Helper;
using Data.DataContext;
using Data.DataModels;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;
using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Implementation
{
    public class HomeFunction : IHomeFunction
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor httpContext;
        public HomeFunction(ApplicationDbContext context,IHttpContextAccessor _httpContext)
        {
            _context = context;
            httpContext = _httpContext;
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
                Mobile = req.Phonenumber,
                Street = req.Requestclients.ElementAt(0).Street,
                City = req.Requestclients.ElementAt(0).City,
                State = req.Requestclients.ElementAt(0).State,
                Zipcode = req.Requestclients.ElementAt(0).Zipcode,
                Createddate = DateTime.Now,
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
        public PatientReqSubmit CreateAccount(string id)
        {
            PatientReqSubmit patientReqSubmit = new PatientReqSubmit();
            int id2 = int.Parse(EncryptDecryptHelper.Decrypt(id));
            patientReqSubmit.reqclientid = id;
            patientReqSubmit.Email = _context.Requests.FirstOrDefault(u => u.Requestid == id2)!.Email;
            return patientReqSubmit;
        }
        public ChatBoxModal ChatwithProvider(int phyid)
        {
            ChatBoxModal modal = new ChatBoxModal();
            var phy = _context.Physicians.FirstOrDefault(u=>u.Physicianid == phyid);
            modal.photo = phy?.Photo??"";
            modal.sendtoname = phy.Firstname;
            modal.sendtoaspid = phy.Aspnetuserid!;
            var jwtservice = httpContext.HttpContext!.RequestServices.GetService<IJwtRepository>();
            var request = httpContext.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice!.ValidateToken(token!, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId")!.Value;
            modal.thisaspid = roleClaim;
            return modal;
        }
        public ChatBoxModal ChatwithAdmin(int sendaerid, int receiverid)
        {
            ChatBoxModal modal = new ChatBoxModal();
            var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == receiverid.ToString());
            modal.sendtoname = admin.Firstname;
            modal.sendtoaspid = receiverid.ToString();
            modal.thisaspid = sendaerid.ToString();
            return modal;
        }
        public List<Chathistory> GetAllChat(int sendid,int thisid)
        {
            var allchat = _context.Chathistories.Where(u => (u.Sender == sendid.ToString() && u.Reciever == thisid.ToString()) || (u.Reciever == sendid.ToString() && u.Sender == thisid.ToString())).OrderBy(u=>u.Senttime).ToList();
            return allchat;
        }
    }
}
