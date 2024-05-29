using Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminsBoxModal
    {
        public int sId { get; set; }
        public List<Admin> Admins { get; set; } = new List<Admin>();
    }
}
