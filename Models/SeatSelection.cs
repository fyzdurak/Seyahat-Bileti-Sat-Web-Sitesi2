using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BiletSatisWebApp.Models
{
    public class SeatSelection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; }



        public List<string> SelectedSeats { get; set; } = new List<string>();

        public DateTime SelectionDate { get; set; } = DateTime.Now;
    }
}


