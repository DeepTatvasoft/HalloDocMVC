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
}
