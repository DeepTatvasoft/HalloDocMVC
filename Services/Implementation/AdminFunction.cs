using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class AdminFunction : IAdminFunction
    {
        private readonly ApplicationDbContext _context;

        public AdminFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public (bool, string) loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj != null)
            {
                var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == obj.Id.ToString());
                if (admin == null)
                {
                    return (false, null);
                }
                else
                {
                    return (true, obj.Username);
                }
            }
            else
            {
                return (false, null);
            }
        }
        public NewStateData AdminDashboarddata(int status1,int status2,int status3)
        {
            NewStateData data = new NewStateData();
            List<Request> req = _context.Requests.Include(r=>r.Requestclients).Where(u => u.Status == status1 || u.Status == status2 || u.Status==status3).ToList();
            foreach(var item in req)
            {
                List<Requestclient> rec = item.Requestclients.ToList();
            }
            data.req = req;
            return data;
        }
    }
}
