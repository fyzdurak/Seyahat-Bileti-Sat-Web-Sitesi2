using BiletSatisWebApp.Models;
namespace BiletSatisWebApp.Models.ViewModel
{

    public class KoltukSecimiVM
    {
        public Trip Trip { get; set; }
        public List<int> SelectedSeats { get; set; }
    }

}