using Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminProfile 
    {
        public string? Username { get; set; }
        public Admin? admindata { get; set; }
        public List<Region>? regions { get; set; }
        public string? ResetPassword { get; set; }
    }
}
