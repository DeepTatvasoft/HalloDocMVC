using Microsoft.AspNetCore.Http;

namespace Services.ViewModels
{
    public class PatientReqSubmit
    {
        public string? Symptoms { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? Room { get; set; }
        public string? reqclientid { get; set; }
        public List<IFormFile>? Upload { get; set; }
    }
}
