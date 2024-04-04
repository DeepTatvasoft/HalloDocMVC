using Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class Scheduling
    {
        public List<Region> regions { get; set; }
        public int regionid { get; set; }
        public int physicianid { get; set; }
        public DateTime shiftdate { get; set; }
        public TimeOnly starttime { get; set; }
        public TimeOnly endtime { get; set; }
        public int repeatcount { get; set; }
        public string physicianname { get; set; }
        public string modaldate { get; set; }
        public int shiftdetailid { get; set; }
        public string curdate { get; set; }
        public string wisetype { get; set; }
        public List<Physician> Phyoncall { get; set; } 
        public List<Physician> Phyoffduty { get; set; }
        public List<int> oncall { get; set; }
        public List<int> offcall { get; set; }
    }
    public class DayWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }
    }
    public class MonthWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }

    }
    public class WeekWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }

    }
    public class ShiftforReviewModal
    {
        public List<Region> regions { get; set; }
        public List<Shiftdetail> shiftdetail { get; set; }
        public int totalpages { get; set; }
        public int currentpage { get; set; }
        public int regionid { get; set; }
    }

}
