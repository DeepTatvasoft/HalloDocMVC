using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class BusinessSubmit
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string? BusFirstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string? BusLastname { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? BusPhonenumber { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string? BusEmail { get; set; }

        public string? BusProperty { get; set; }

        public string? BusCaseNum { get; set; }

        public string? PatSymptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string? PatFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string? PatLastName { get; set; }

        [Required(ErrorMessage = "*Date Of Birth is required")]
        public DateOnly PatDOB { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string? PatEmail { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? PatPhoneNumber { get; set; }

        [Required]
        public string? PatStreet { get; set; }

        public string? PatCity { get; set; }

        public string? PatState { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string? PatZipcode { get; set; }

        public string? PatRoom { get; set; }

        public IFormFile? Upload { get; set; }

    }
}
