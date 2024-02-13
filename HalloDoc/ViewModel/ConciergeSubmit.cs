namespace HalloDoc.ViewModel
{
    public class ConciergeSubmit
    {
        public string ConFirstName { get; set; }
        public string ConLastName { get; set; }
        public string ConPhonenumber { get; set; }
        public string ConEmail { get; set; }
        public string ConProperty { get; set; }
        public string ConStreet { get; set; }
        public string ConCity { get; set; }
        public string ConState { get; set; }
        public string ConZipcode { get; set; }
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
