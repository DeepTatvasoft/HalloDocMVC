using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class LoggedInPersonViewModel
    {
        public int userid { get; set; }
        public int aspuserid { get; set; }
        public string username { get; set; }
        public string role { get; set; }
    }
}
