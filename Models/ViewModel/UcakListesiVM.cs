namespace BiletSatisWebApp.Models.ViewModel
{
    public class UcakListesiVM
    {
        public List<Trip> Trips { get; set; }
        public string? Nereden { get; set; }
        public string? Nereye { get; set; }
        public DateTime? GidisTarihi { get; set; }
    }
}
