using Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PartnersModal
    {
        public List<Healthprofessional> healthprofessionals {  get; set; }
        public List<Healthprofessionaltype> healthprofessionaltypes { get; set; }
    }
    public class AddBusinessModal
    {
        public int vendorid { get; set; }
        public List<Healthprofessionaltype> healthprofessionaltypes { get; set; }

        [Required(ErrorMessage = "*Business Name is required")]
        public string businessname { get; set; }

        [Required]
        public int professiontype { get; set; }
        [Required]
        public string faxnumber { get; set; }

        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{9})$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string phonenumber { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string email { get; set; }
        public string buscontact { get; set; }

        [Required]
        public string street { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string state { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string zipcode { get; set; }
    }
}
