using HalloDoc.Models;
namespace HalloDoc.ViewModel
{
    public class PatientDashboardedit
    {
        public User User { get; set; }
        public List<Request> requests { get; set; }
        public DateTime tempdate { get; set; }
        public List<Requestwisefile> requestwisefiles { get; set; }
        public List<IFormFile> Upload { get; set; }
        public string uploader { get; set; }
    }
}
