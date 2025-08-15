using System.Collections.Generic;

namespace BiletSatisWebApp.Models
{
    public class ProfileViewModel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public List<Trip> Trips { get; set; }
    }
}
