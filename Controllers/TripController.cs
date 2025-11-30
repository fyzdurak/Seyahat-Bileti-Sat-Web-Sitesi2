using BiletSatisWebApp.Data;
using BiletSatisWebApp.Models;
using BiletSatisWebApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class TripController : Controller
{
    private readonly ApplicationDbContext _context;

    public TripController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Koltuk seçimi sayfası
    public IActionResult SelectSeats(int id)
    {
        var trip = _context.Trips.Find(id);
        if (trip == null) return NotFound();

        if (trip.Seats == null || trip.Seats.Count == 0)
            trip.Seats = new List<string> { "A1", "A2", "A3", "B1", "B2", "B3" };

        var model = new KoltukSecimiVM
        {
            Trip = trip,
            SeatSelection = new SeatSelection()
        };

        return View(model);
    }

    // POST: Seçilen koltukları session'a kaydet (örnek, isteğe bağlı)
    [HttpPost]
    public IActionResult SelectSeatsPost(int tripId, [FromBody] SeatSelection seatSelection)
    {
        HttpContext.Session.SetString("SelectedSeats", JsonSerializer.Serialize(seatSelection));
        return Json(new { success = true });
    }

    // Ödeme sayfası
   /* public IActionResult Odeme(int id)
    {
        var trip = _context.Trips.Find(id);
        if (trip == null) return NotFound();

        if (trip.Seats == null || trip.Seats.Count == 0)
            trip.Seats = new List<string> { "A1", "A2", "A3", "B1", "B2", "B3" };

        var bookedSeats = trip.BookedSeats ?? new List<string>();

        var sessionSeats = HttpContext.Session.GetString("SelectedSeats");
        SeatSelection selectedSeats = string.IsNullOrEmpty(sessionSeats)
            ? new SeatSelection()
            : JsonSerializer.Deserialize<SeatSelection>(sessionSeats);

        // KONTROL: Seçilen koltuk listesini alıyoruz
        var selectedSeatsList = selectedSeats?.SelectedSeats ?? new List<string>();

        var model = new OdemeVM
        {
            TripId = trip.Id,
            FromCity = trip.FromCity,
            ToCity = trip.ToCity,
            Price = trip.Price,
            Seats = trip.Seats,
            BookedSeats = bookedSeats,
            SelectedSeatsJsonString = System.Text.Json.JsonSerializer.Serialize(selectedSeatsList)
        };

        return View(model);
    }*/

    // Ödeme tamamla: tek [HttpPost] atributu, concurrency kontrolü ekli
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CompletePayment([FromBody] CompletePaymentVM vm)
    {
        if (vm == null) return BadRequest("Geçersiz istek.");
        var trip = _context.Trips.Find(vm.TripId);
        if (trip == null) return NotFound();

        if (vm.PaidSeats == null || !vm.PaidSeats.Any())
            return BadRequest("Hiç koltuk seçilmedi!");

        // DB'deki mevcut BookedSeats'i al
        var currentSeats = trip.BookedSeats?.ToList() ?? new List<string>();

        // Çakışma kontrolü
        var alreadyBooked = currentSeats.Intersect(vm.PaidSeats).ToList();
        if (alreadyBooked.Any())
        {
            return Conflict(new
            {
                success = false,
                message = "Aşağıdaki koltuklar zaten rezerve edilmiş: " + string.Join(", ", alreadyBooked)
            });
        }

        // Yeni koltukları ekle
        currentSeats.AddRange(vm.PaidSeats);
        trip.BookedSeats = currentSeats; // otomatik olarak JSON serialize olur

        _context.SaveChanges();

        HttpContext.Session.Remove("SelectedSeats");

        return Json(new { success = true });
    }

}






