namespace BiletSatisWebApp.Models.ViewModel
{
    public class OtobusListesiVM
    {
        public List<Trip> Trips{ get; set; }
        public string? Nereden { get; set; }
        public string? Nereye { get; set; }
        public DateTime? GidisTarihi { get; set; }
    }
}
