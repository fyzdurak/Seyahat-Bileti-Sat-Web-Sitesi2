using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiletSatisWebApp.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FromCity { get; set; }

        [Required]
        public string ToCity { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }


        [Required]
        public string TransportType { get; set; } // Otobüs, Uçak, Tren

        public DateTime SaleDate { get; set; }  //Satışa çıkarıldığı tarih

        [Required]
        public int TicketCount { get; set; }  // Satışa çıkarılan bilet sayısı

        public int TicketsSold { get; set; } = 0; // Satılan bilet

        public decimal Price { get; set; }

    }
}

