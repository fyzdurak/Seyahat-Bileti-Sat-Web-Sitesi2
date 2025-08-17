using System;
using System.Collections.Generic;

namespace BiletSatisWebApp.Models.ViewModel
{
    public class SeatSelectionRequestVM
    {
        public int TripId { get; set; }
        public List<int> SelectedSeats { get; set; }
    }
}
