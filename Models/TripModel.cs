using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BiletSatisWebApp.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }   // TripId yerine standart Id kullanıyoruz

        [Required]
        public string FromCity { get; set; }   // Kalkış şehri

        [Required]
        public string ToCity { get; set; }     // Varış şehri

        [Required]
        public DateTime DepartureDate { get; set; }   // Tarih + saat

        [Required]
        public TimeSpan DepartureTime { get; set; }
        [Required]
        public string TransportType { get; set; }
        [Required]
        public int TicketCount { get; set; }

        [Required]
        public decimal Price { get; set; }     // Bilet ücreti
        public DateTime? SaleDate { get; set; }
        public string? SeatLayout { get; set; }
        public int TicketsSold { get; set; } = 0;


        // DB'de JSON string olarak saklanacak
        public string BookedSeatsJson { get; set; } = "[]";

        [NotMapped]
        public List<string> BookedSeats
        {
            get => JsonSerializer.Deserialize<List<string>>(BookedSeatsJson ?? "[]");
            set => BookedSeatsJson = JsonSerializer.Serialize(value);
        }

        // ⬇ Koltuk listesi (bunu DB'de tutmuyoruz)
        [NotMapped]
        public List<string> Seats { get; set; } = new List<string>
        {
            "A1","A2","A3",
            "B1","B2","B3",
            "C1","C2","C3"
        };
    }
}



