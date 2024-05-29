using Data.DataModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class StudentFormModal
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }
        [Required]
        public string grade { get; set; }
        [Required]
        public string course { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public List<Student> students { get; set; }
        public string gender { get; set; }
        public int studentid { get; set; }
    }
}
