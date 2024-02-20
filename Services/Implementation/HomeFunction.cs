using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
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
        public (Aspnetuser,User) ValidateUser([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj == null)
            {
                return (null,null);
            }
            var user = _context.Users.FirstOrDefault(u => u.Aspnetuserid == obj.Id);
            return (obj,user);
        }
    }
}
