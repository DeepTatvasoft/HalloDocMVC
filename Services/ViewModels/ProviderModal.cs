using Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ProviderModal
    {
        public List<Physician>? physicians { get; set; }
        public List<Region>? regions { get; set; }
        public List<int> physiciannotificationid { get; set; }
    }
}
