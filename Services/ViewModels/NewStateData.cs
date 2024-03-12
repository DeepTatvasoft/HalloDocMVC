using Data.DataModels;
using Microsoft.AspNetCore.Http;

namespace Services.ViewModels
{
    public class NewStateData
    {
        public List<Request> req { get; set; }
        public List<Requestclient> requestclients { get; set; }
        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int concludecount { get; set; }
        public int Toclosecount { get; set; }
        public int Unpaidcount { get; set; }
        public List<Region> regions { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Casetag> casetags { get; set; }
        public List<Requeststatuslog> requeststatuslogs { get; set; }
        public int totalpages { get; set; }
        public int reqid { get; set; }
        public string phonenumber { get; set; }
        public string emaill { get; set; }
    }
    public class NewStateData1
    {
        public Request req { get; set; }
        public DateTime DateOnly { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public string room { get; set; }
        public string symptoms { get; set; }
        public List<Region> regions { get; set; }

    }
    public class ViewNotesModel
    {
        public Requeststatuslog requeststatuslogs { get; set; }
        public string adminname { get; set; }
        public string phyname { get; set; }
        public string adminnotes { get; set; }
        public int reqid { get; set; }
    }
    public class AdminviewDoc
    {
        public string Username { get; set; }
        public List<IFormFile> Upload { get; set; }
        public List<Requestwisefile> reqfile { get; set; }
        public string ConfirmationNum { get; set; }
        public int reqid { get; set; }
    }
    public class SendOrders
    {
        public int reqid { get; set; }
        public int vendorid { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string prescription { get; set; }
        public int refil { get; set; }
        public int professionid { get; set;}
        public string createdby { get; set;}
    }
    public class Agreementmodal
    {
        public int reqid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string reason { get; set; }
    }
}
