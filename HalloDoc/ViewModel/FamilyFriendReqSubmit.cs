namespace HalloDoc.ViewModel
{
    public class FamilyFriendReqSubmit
    {
        public string FamFirstName { get; set; }
        public string FamLastName { get; set; }
        public string FamMobile { get; set; }
        public string FamEmail { get; set; }
        public string FamRelation { get; set; }
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
