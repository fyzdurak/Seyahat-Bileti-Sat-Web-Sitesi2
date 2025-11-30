using BiletSatisWebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiletSatisWebApp.Models.ViewModel

{
    public class OdemeVM
    {
        public int TripId { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public decimal Price { get; set; }
        public decimal OdemeTutari { get; set; } 
        public List<string> Seats { get; set; } = new List<string>();
        public List<string> BookedSeats { get; set; } = new List<string>();
        [NotMapped]
        public SeatSelection SeatSelection { get; set; } = new SeatSelection();
        
        public List<string> SelectedSeats { get; set; } = new List<string>();
        public Trip Trip { get; set; } 
    }

}


