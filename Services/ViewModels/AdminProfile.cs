using Data.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class AdminProfile 
    {
        public int adminid { get; set; }

        [Required]
        public string? Username { get; set; }
        public List<Region>? regions { get; set; }
        public List<Role>? roles { get; set; }
        public string? ResetPassword { get; set; }
        public HashSet<Region>? adminregions { get; set; }
        public int status { get; set; }

        [Required]
        public int? roleid { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string? firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string? lastname { get; set;}

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string? email { get; set; }

        [Compare("email")]
        public string? confirmemail { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string? phonenumber { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }

        [Required]
        public string? city { get; set; }

        [Required]
        public int? regionid { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string? zipcode { get; set;}

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string? altphone { get; set; }

    }
}
