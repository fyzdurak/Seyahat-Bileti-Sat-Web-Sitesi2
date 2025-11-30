namespace BiletSatisWebApp.Models.ViewModel
{
    public class CompletePaymentVM
    {
        public int TripId { get; set; }
        public List<string> PaidSeats { get; set; } = new List<string>();
    }
}
