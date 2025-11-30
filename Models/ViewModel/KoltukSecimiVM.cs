using System.Collections.Generic;

namespace BiletSatisWebApp.Models.ViewModel
{
    public class KoltukSecimiVM
    {
        public Trip Trip { get; set; }
        public SeatSelection SeatSelection { get; set; } = new SeatSelection();
    }
}

