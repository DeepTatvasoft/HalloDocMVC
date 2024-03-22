using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class PatientReqSubmit
    {
        public string? Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "*Date Of Birth is required")]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string? Zipcode { get; set; }

        public string? Room { get; set; }

        public string? reqclientid { get; set; }

        public List<IFormFile>? Upload { get; set; }

        public string? AdminNotes { get; set; }
    }
}
