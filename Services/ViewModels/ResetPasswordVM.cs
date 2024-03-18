using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ResetPasswordVM
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
    }
}
