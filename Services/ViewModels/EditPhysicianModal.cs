using Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class EditPhysicianModal
    {
        public Physician physician { get; set; }
        public string? Username { get; set; }
        public List<Region> regions { get; set; }
        public HashSet<Region> physicianregions { get; set; }
        public string password { get; set; }
    }
}
