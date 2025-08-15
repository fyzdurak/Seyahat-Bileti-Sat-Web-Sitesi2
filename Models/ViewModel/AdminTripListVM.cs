using System.Collections.Generic;

namespace BiletSatisWebApp.Models
{
    public class AdminTripListVM
    {
        public List<Trip> UpcomingTrips { get; set; } // Yaklaşan seferler
        public List<Trip> PastTrips { get; set; }     // Geçmiş seferler
    }
}
