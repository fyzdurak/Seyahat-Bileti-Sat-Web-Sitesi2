namespace BiletSatisWebApp.Models.ViewModel
{
    public class TrenListesiVM
    {
        public List<Trip> Trips { get; set; }
        public string? Nereden { get; set; }
        public string? Nereye { get; set; }
        public DateTime? GidisTarihi { get; set; }
    }
}
