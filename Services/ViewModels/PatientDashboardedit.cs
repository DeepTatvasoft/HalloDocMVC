using Data.DataModels;
using Microsoft.AspNetCore.Http;
namespace Services.ViewModels
{
    public class PatientDashboardedit
    {
        public User User { get; set; }
        public List<Request> requests { get; set; }
        public DateTime tempdate { get; set; }
        public List<Requestwisefile> requestwisefiles { get; set; }
        public List<IFormFile> Upload { get; set; }
        public string uploader { get; set; }
        public int reqid { get; set; }
        public string filename { get; set; }
        public List<string> allfile { get; set; } 
        
    }
}
