using Data.DataModels;

namespace Services.ViewModels
{
    public class AdminProfile 
    {
        public string? Username { get; set; }
        public Admin? admindata { get; set; }
        public List<Region>? regions { get; set; }
        public string? ResetPassword { get; set; }
        public HashSet<Region>? adminregions { get; set; }
    }
}
