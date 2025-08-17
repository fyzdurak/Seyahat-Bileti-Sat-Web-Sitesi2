using BiletSatis.Models;
using BiletSatisWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BiletSatisWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<SeatSelection> SeatSelections{ get; set; }
        public DbSet<IletisimMesaj> IletisimMesajlar { get; set; }
    }
}
