using Data.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PayrateModal
    {
        public Payrate payrates {  get; set; }
        public int phyid { get; set; }
    }
    public class ProviderFinalizeTimeSheetModal
    {
        public List<TimeSheetData> Rows { get; set; }
        public List<ReceiptsData> receiptsDatas { get; set; }
        public DateTime DropDate { get; set; }
        public DateTime receiptsdate { get; set; }
        public ReceiptsData showreceiptsdata { get; set; }
        public bool isfinalized { get; set; } 
        public bool isapproved { get; set; } 
        public int biweekid { get;set; }
        public int physicianid { get; set; }
        public List<Physician> physicians { get; set; }
        public Biweektime biweektime { get; set; }
        public string phyname { get; set; }
    }
    public class TimeSheetData
    {
        public DateTime Date { get; set; }
        public int? OnCallHours { get; set; }
        public bool IsHoliday { get; set; } = false;
        public int? NumberOfHouseCalls { get; set;}
        public int? NumberOfPhoneConsults { get; set;}
        public int? TotalHours { get; set;}
    }
    public class ReceiptsData
    {
        public DateTime Date { get; set; }
        public string? itemname { get; set; }
        public int? amount { get; set; }
        public IFormFile? bill { get; set; }
        public string? filename { get;set; }
    }
}
