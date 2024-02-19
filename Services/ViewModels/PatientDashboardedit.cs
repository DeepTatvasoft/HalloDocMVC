using Data.DataModels;
using Microsoft.AspNetCore.Http;
namespace Services.ViewModel
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
        public String filename { get; set; }
        public List<String> allfile { get; set; } 
        
    }
}
