using System;
using System.Collections.Generic;

namespace BiletSatisWebApp.Models
{
    public class SeatSelection
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public List<int> SelectedSeats { get; set; }
        public DateTime SelectionDate { get; set; } = DateTime.Now;
    }
}