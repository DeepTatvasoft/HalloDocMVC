using Microsoft.AspNetCore.Http;

namespace Services.ViewModel
{
    public class BusinessSubmit
    {
        public string BusFirstname { get; set; }
        public string BusLastname { get; set; }
        public string BusPhonenumber { get; set; }
        public string BusEmail { get; set; }
        public string BusProperty { get; set; }
        public string BusCaseNum { get; set; }
        public string PatSymptoms { get; set; }
        public string PatFirstName { get; set; }
        public string PatLastName { get; set; }
        public DateOnly PatDOB { get; set; }
        public string PatEmail { get; set; }
        public string PatPhoneNumber { get; set; }
        public string PatStreet { get; set; }
        public string PatCity { get; set; }
        public string PatState { get; set; }
        public string PatZipcode { get; set; }
        public string PatRoom { get; set; }
        public IFormFile? Upload { get; set; }

    }
}
