using Data.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class EditPhysicianModal
    {
        public int physicianid { get; set; }
        public string? Username { get; set; }
        public List<Region> regions { get; set; }
        public HashSet<Region> physicianregions { get; set; }

        [Required]
        public string? password { get; set; }
        public List<Role> roles { get; set; }

        [Required]
        public int? role { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string? Firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string? Email { get; set; }
        public int? status { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? phonenumber { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string? altphone { get; set; }
        public string? license { get; set; }
        public string? npi { get; set; }
        public string? synemail { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public int? State { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string? zipcode { get; set; }

        [Required]
        public string? BusinessName { get; set; }

        [Required]
        public string? Businesssite { get; set; }
        public string? Adminnotes { get; set; }
        public int regionid { get; set; }
        public string? sign { get; set; }
        public BitArray? Isagreementdoc { get; set; }
        public BitArray? Isbackgrounddoc { get; set; }
        public BitArray? Iscredentialdoc { get; set; }
        public BitArray? Isnondisclosuredoc { get; set; }
        public BitArray? Islicensedoc { get; set; }
        public IFormFile Images { get; set; }
        public string photo { get; set; }
        public IFormFile ICAdoc { get; set; }
        public IFormFile BGcheckdoc { get; set; }
        public IFormFile HIPAAdoc { get; set; }
        public IFormFile NDdoc { get; set; }
        public IFormFile LDdoc { get; set; }
    }
}
