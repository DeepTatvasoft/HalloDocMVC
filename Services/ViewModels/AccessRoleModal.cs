using Data.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class AccessRoleModal
    {
        public List<Menu> menu { get; set; }
        public string RoleName { get; set; }
        [Required]
        public int accountType { get; set; }
        public List<Role> roles { get; set; }
        public List<int> selectedmenuid { get; set; }
        public int roleid { get; set; }
    }
}
