using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
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
        public bool loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == obj.Id.ToString());
            if (obj == null || admin == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
