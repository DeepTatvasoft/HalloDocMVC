using Data.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class AccessRoleModal
    {
        public List<Menu>? menu { get; set; }
        public string? RoleName { get; set; }
        [Required]
        public int accountType { get; set; }
        public List<Role>? roles { get; set; }
        public List<int>? selectedmenuid { get; set; }
        public int roleid { get; set; }
    }
    public class UserAccessModal
    {
        public List<Aspnetrole>? aspnetroles { get; set; }
        public List<Aspnetuser>? aspnetusers { get; set; } 
        public List<Aspnetuserrole>? aspnetuserroles { get; set; }
        public int usercount { get; set; }
        public int admincount { get; set; }
        public int phycount { get; set; }
        public List<Request>? req { get; set; }
    }
}
