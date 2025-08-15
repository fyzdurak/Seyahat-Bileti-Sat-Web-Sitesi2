using Microsoft.AspNetCore.Identity;

namespace BiletSatis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string UserType { get; set; }
    }
}
