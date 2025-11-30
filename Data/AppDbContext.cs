using BiletSatis.Models;
using BiletSatisWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BiletSatisWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        // ApplicationDbContext.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Virgüle ayrılmış string olarak kaydetmek ve okumak için
            var listConverter = new ValueConverter<List<string>, string>(
                v => string.Join(",", v),         // List<string> -> string'e dönüştürme (kaydetme)
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() // string -> List<string>'e dönüştürme (okuma)
            );

            modelBuilder.Entity<SeatSelection>()
                .Property(s => s.SelectedSeats)
                .HasConversion(listConverter); // Converter'ı ata
        }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<SeatSelection> SeatSelections{ get; set; }
        public DbSet<IletisimMesaj> IletisimMesajlar { get; set; }
    }
}
