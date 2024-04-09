using Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class RecordstabModal
    {
        public List<User> user { get; set; }
        public int totalpages { get; set; }
        public int currentpage { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
        public string? phonenumber { get; set; }
    }
    public class BlockHistoryModal
    {
        public List<Blockrequest> blockrequests { get; set; }
        public List<Requestclient> reqclient { get; set; }
        public int totalpages { get; set; }
        public int currentpage { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public DateTime? date { get; set; }
        public string? phonenumber { get; set; }
    }

    public class EmailLogsModal
    {
        public int? roleid { get; set; }
        public string? receivername { get; set; }
        public string? email { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? sentdate { get; set; }
        public int totalpages { get; set; }
        public int currentpage { get; set; }
        public List<Aspnetrole> aspnetrole { get; set; }
        public List<Emaillog> emaillogs { get; set; }
    }

    public class SearchRecordModal
    {
        public int? reqstatus { get; set; }
        public string? patientname { get; set; }
        public int? reqtype { get; set; }
        public DateTime? fromDOS { get; set; }
        public DateTime? toDOS { get; set; }
        public string? providername { get; set; }
        public string? email { get; set; }
        public string? phonenumber { get; set; }
        public int totalpages { get; set; }
        public int currentpage { get; set; }
        public List<Request> req { get; set; }
    }

    public class ExploreModal
    {
        public List<Requestclient> reqclient { get; set; }
    }
}
