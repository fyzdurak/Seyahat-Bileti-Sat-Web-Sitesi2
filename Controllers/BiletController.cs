using BiletSatisWebApp.Data;
using BiletSatisWebApp.Models;
using BiletSatisWebApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiletSatisWebApp.Controllers
{
    public class BiletController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiletController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ana sayfa
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Trips
                .Where(t => t.SaleDate != null)
                .ToListAsync();

            return View(tickets);
        }

        #region === Yardımcı Metotlar ===

        private async Task<List<string>> GetCitiesByTransportTypeAsync(string transportType)
        {
            // 1. SQL’de çekilebilen kısım
            var tripList = await _context.Trips
                .Where(t => t.TransportType == transportType)
                .Select(t => new { t.FromCity, t.ToCity })
                .ToListAsync();

            // 2. Bellekte açıyoruz
            var cities = tripList
                .SelectMany(t => new[] { t.FromCity, t.ToCity })
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            if (!cities.Any())
                cities = new List<string> { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" };

            return cities;
        }


        private async Task<List<Trip>> GetTripsAsync(string transportType, string from, string to, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Trips
                .Where(t =>
                    t.TransportType == transportType &&
                    t.FromCity == from &&
                    t.ToCity == to &&
                    t.DepartureDate >= start &&
                    t.DepartureDate < end)
                .ToListAsync();
        }


        private async Task<Trip?> GetTripByIdAsync(int id)
        {
            return await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
        }

        private async Task<SeatSelection?> GetSeatSelectionByIdAsync(int id)
        {
            return await _context.SeatSelections.FirstOrDefaultAsync(s => s.Id == id);
        }

        private async Task<List<SeatSelection>> GetSelectedSeatsByTripIdAsync(int tripId)
        {
            return await _context.SeatSelections
                .Where(s => s.TripId == tripId)
                .ToListAsync();
        }

        private async Task<int> SaveSelectedSeatsAsync(int tripId, List<int> selectedSeats)
        {
            var existingSelection = await _context.SeatSelections.FirstOrDefaultAsync(s => s.TripId == tripId);

            if (existingSelection != null)
            {
                // 2. Eğer varsa, koltuk listesini GÜNCELLE
                existingSelection.SelectedSeats = selectedSeats.Select(s => s.ToString()).ToList();
                existingSelection.SelectionDate = DateTime.Now;
                _context.SeatSelections.Update(existingSelection);
                await _context.SaveChangesAsync();
                return existingSelection.Id;
            }
            else
            {
                // 3. Yoksa, YENİ KAYIT OLUŞTUR
                var seatSelection = new SeatSelection
                {
                    TripId = tripId,
                    SelectedSeats = selectedSeats.Select(s => s.ToString()).ToList(),
                    SelectionDate = DateTime.Now
                };

                _context.SeatSelections.Add(seatSelection);
                await _context.SaveChangesAsync();
                return seatSelection.Id;
            }
        }


        #endregion

        #region === Otobüs ===

        [HttpGet]
        public async Task<IActionResult> Otobus()
        {
            ViewBag.Cities = await GetCitiesByTransportTypeAsync("Otobüs");
            return View();
        }

        [HttpPost]
        public IActionResult Otobus(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            return RedirectToAction("OtobusListesi", new
            {
                from = Nereden,
                to = Nereye,
                tarih = GidisTarihi.ToString("dd.MM.yyyy")
            });
        }

        public async Task<IActionResult> OtobusListesi(string from, string to, string tarih)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(tarih))
                return View();

            DateTime dt = Convert.ToDateTime(tarih);
            var trips = await GetTripsAsync("Otobüs", from, to, dt);

            var vm = new OtobusListesiVM
            {
                Trips = trips,
                Nereden = from,
                Nereye = to,
                GidisTarihi = dt
            };

            return View(vm);
        }

        #endregion

        #region === Koltuk Seçimi ===

        [HttpGet]
        public async Task<IActionResult> KoltukSecimi(int tripId)
        {
            var trip = await GetTripByIdAsync(tripId);
            var seatSelection = (await GetSelectedSeatsByTripIdAsync(tripId)).FirstOrDefault() ?? new SeatSelection();


            if (trip == null)
                return Json(new { success = false, message = "Sefer bulunamadı!" });

            var vm = new KoltukSecimiVM
            {
                Trip = trip,
                SeatSelection = seatSelection
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> KoltukSecimi([FromBody] SeatSelectionRequest request)
        {
            var trip = await GetTripByIdAsync(request.TripId);
            if (trip == null)
                return Json(new { success = false, message = "Sefer bulunamadı!" });

            var selectionId = await SaveSelectedSeatsAsync(request.TripId, request.SelectedSeats);

            return Json(new { success = true, message = "Koltuk seçimi tamamlandı!", selectionId });
        }

        #endregion

        #region === Ödeme ===

        
        [HttpGet("/bilet/odeme/{id}")]
        public async Task<IActionResult> Odeme(int id)
        {
            var seatSelection = await GetSeatSelectionByIdAsync(id);
            if (seatSelection == null)
                return NotFound("Seçim bulunamadı!");

            var trip = await GetTripByIdAsync(seatSelection.TripId);
            if (trip == null)
                return NotFound("Sefer bulunamadı!");
            var selectedSeatsList = seatSelection?.SelectedSeats?.Select(s => s).ToList() ?? new List<string>();
            
            var vm = new OdemeVM
            {
                TripId = trip.Id,
                FromCity = trip.FromCity,
                ToCity = trip.ToCity,
                Price = trip.Price,

                // Seats: int üretiyorduk, string bekleniyorsa string'e çevir
                Seats = Enumerable.Range(1, trip.TicketCount)
                      .Select(x => x.ToString())
                      .ToList(),

                // BookedSeats zaten string listesi ise direkt kullan; null ise boş string listesi
                BookedSeats = trip.BookedSeats ?? new List<string>(),

                //SelectedSeatsJsonString = System.Text.Json.JsonSerializer.Serialize(selectedSeatsList),
                SelectedSeats = selectedSeatsList
            };


            return View(vm);
        }


        #endregion

        #region === Uçak ===

        [HttpGet]
        public async Task<IActionResult> Ucak()
        {
            ViewBag.Cities = await GetCitiesByTransportTypeAsync("Uçak");
            return View();
        }

        [HttpPost]
        public IActionResult Ucak(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            return RedirectToAction("UcakListesi", new
            {
                from = Nereden,
                to = Nereye,
                tarih = GidisTarihi.ToString("dd.MM.yyyy")
            });
        }

        public async Task<IActionResult> UcakListesi(string from, string to, string tarih)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(tarih))
                return View();

            DateTime dt = Convert.ToDateTime(tarih);
            var trips = await GetTripsAsync("Uçak", from, to, dt);

            var vm = new UcakListesiVM
            {
                Trips = trips,
                Nereden = from,
                Nereye = to,
                GidisTarihi = dt
            };

            return View(vm);
        }

        #endregion

        #region === Tren ===

        [HttpGet]
        public async Task<IActionResult> Tren()
        {
            ViewBag.Cities = await GetCitiesByTransportTypeAsync("Tren");
            return View();
        }

        [HttpPost]
        public IActionResult Tren(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            return RedirectToAction("TrenListesi", new
            {
                from = Nereden,
                to = Nereye,
                tarih = GidisTarihi.ToString("dd.MM.yyyy")
            });
        }

        public async Task<IActionResult> TrenListesi(string from, string to, string tarih)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(tarih))
                return View();

            DateTime dt = Convert.ToDateTime(tarih);
            var trips = await GetTripsAsync("Tren", from, to, dt);

            var vm = new TrenListesiVM
            {
                Trips = trips,
                Nereden = from,
                Nereye = to,
                GidisTarihi = dt
            };

            return View(vm);
        }

        #endregion
    }
}

