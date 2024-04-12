using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class EncounterFormViewModel
    {
        public int RequestId { get; set; }
        public string role { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string? Firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string? Lastname { get; set; }

        public string? Location { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime? Dateofservice { get; set; }

        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Mobile number format is not valid")]
        public string? Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? HistoryOfIllness { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? MedicalHistory { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Medication { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Allergies { get; set; }

        [Required]
        public decimal? Temp { get; set; }
        [Required]
        public decimal? HR { get; set; }
        [Required]
        public decimal? RR { get; set; }
        [Required]
        public int? BPs { get; set; }
        [Required]
        public int? BPd { get; set; }
        [Required]
        public decimal? O2 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Pain { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Heent { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? CV { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Chest { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? ABD { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Extr { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Skin { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Neuro { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Other { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Diagnosis { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? TreatmentPlan { get; set; }
            
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? MedicationsDispended { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Procedure { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string? Followup { get; set; }

        public bool isFinaled { get; set; }
    }
}

