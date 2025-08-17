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

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Trips
                .Where(t => t.SaleDate != null)
                .ToListAsync();

            return View(tickets);
        }

        [HttpGet]
        public IActionResult Otobus()
        {
            var fromCities = _context.Trips
                .Where(t => t.TransportType == "Otobüs") // sadece otobüs seferleri
                .Select(t => t.FromCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList(); // DB'den çekme

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Otobüs") // sadece otobüs seferleri
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList(); // DB'den çekme

            // Birleştir ve tekrar distinct yapma
            var cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Hiç sefer yoksa fallback şehir listesi
            if (!cities.Any())
            {
                cities = new List<string> { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" };
            }

            ViewBag.Cities = cities;
            return View();
        }

        [HttpPost]
        public IActionResult Otobus(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            var fromCities = _context.Trips
                .Where(t => t.TransportType == "Otobüs")
                .Select(t => t.FromCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Otobüs")
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            ViewBag.Cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Arama sonucu
            var trips = _context.Trips
                .Where(t =>
                    t.FromCity == Nereden &&
                    t.ToCity == Nereye &&
                    t.DepartureDate.Date == GidisTarihi.Date)
                .ToList();

            // OtobusListesi sayfasına yönlendir, verileri TempData ile taşı
            //TempData["Trips"] = System.Text.Json.JsonSerializer.Serialize(trips);
            //TempData["Nereden"] = Nereden;
            //TempData["Nereye"] = Nereye;
            //TempData["GidisTarihi"] = GidisTarihi;

            return RedirectToAction("OtobusListesi", new { from = Nereden, to = Nereye, tarih = GidisTarihi.ToString("dd.MM.yyyy") });
        }

        public IActionResult OtobusListesi(string from, string to, string tarih)
        {
            OtobusListesiVM _vm = new OtobusListesiVM();

            //List<Trip> trips = new List<Trip>();
            //if (TempData["Trips"] != null)
            //{
            //    _vm.Trips= JsonSerializer.Deserialize<List<Trip>>(TempData["Trips"].ToString());
            //}

            #region alternatif yol
            List<Trip> trips = new List<Trip>();
            if (from != null && to != null && tarih != null)
            {
                DateTime dt = Convert.ToDateTime(tarih);

                // Arama sonucu
                var tripsResults = _context.Trips
                    .Where(t =>
                        t.FromCity == from &&
                        t.ToCity == to &&
                        t.DepartureDate.Date == dt)
                    .ToList();
                _vm.Trips = tripsResults;
                #endregion

                _vm.Nereden = from;
                _vm.Nereye = to;
                _vm.GidisTarihi = dt;


                return View(_vm);
            }
            else return View();

        }
        [HttpGet]
        public ActionResult KoltukSecimi(int tripId)
        {
            var trip = GetTripById(tripId); // Implement this method to retrieve the trip by ID
            var seatSelection = GetSelectedSeatbyTripID(tripId);

            if (trip == null)
            {
                return Json(new { success = false, message = "Sefer bulunamadı!" });
            }



            var koltukSecimiVM = new KoltukSecimiVM
            {
                Trip = trip,
                SeatSelection= seatSelection
            };

            return View(koltukSecimiVM);
        }

        [HttpPost]
        public ActionResult KoltukSecimi([FromBody] SeatSelectionRequest request)
        {
            var trip = GetTripById(request.TripId); // Implement this method to retrieve the trip by ID
            if (trip == null)
            {
                return Json(new { success = false, message = "Sefer bulunamadı!" });
            }

            // Save the selected seats to the database or session
            var selectionID = SaveSelectedSeats(request.TripId, request.SelectedSeats); // Implement this method to save the selected seats

            return Json(new { success = true, message = "Koltuk seçimi tamamlandı!", selectionId = selectionID });
        }

        private int SaveSelectedSeats(int tripId, List<int> selectedSeats)
        {
            var seatSelection = new SeatSelection
            {
                TripId = tripId,
                SelectedSeats = selectedSeats,
                SelectionDate = DateTime.Now
            };

            _context.SeatSelections.Add(seatSelection);
            _context.SaveChanges();

            return seatSelection.Id;
        }


        private Trip GetTripById(int tripId)
        {
            // Implement logic to retrieve the trip by ID from your data source

            var trip = _context.Trips.Where(p => p.Id == tripId).FirstOrDefault();

            return trip;
            //return new Trip { Id = tripId, FromCity = "Nereden", ToCity = "Nereye", DepartureTime = DateTime.Now.TimeOfDay, Price = 100, TicketCount = 30 };
        }
        private SeatSelection GetSeatSelectionByID(int seatSelectionID)
        {
            // Implement logic to retrieve the trip by ID from your data source

            var seatSelection = _context.SeatSelections.Where(p => p.Id == seatSelectionID).FirstOrDefault();

            return seatSelection;
            //return new Trip { Id = tripId, FromCity = "Nereden", ToCity = "Nereye", DepartureTime = DateTime.Now.TimeOfDay, Price = 100, TicketCount = 30 };
        }
        private List<SeatSelection> GetSelectedSeatbyTripID(int tripID)
        {
            // Implement logic to retrieve the trip by ID from your data source

            var seatSelection = _context.SeatSelections.Where(p => p.TripId == tripID).ToList();

            return seatSelection;
            //return new Trip { Id = tripId, FromCity = "Nereden", ToCity = "Nereye", DepartureTime = DateTime.Now.TimeOfDay, Price = 100, TicketCount = 30 };
        }

        [HttpGet("/bilet/odeme/{id}")]
        public IActionResult Odeme(int id)
        {
            OdemeVM vm = new OdemeVM();
            vm.SeatSelection = GetSeatSelectionByID(id);
            vm.trip = GetTripById(vm.SeatSelection.TripId);
            vm.OdemeTutari=(float)(vm.trip.Price * vm.SeatSelection.SelectedSeats.Count());
            return View(vm);
        }

        [HttpGet]
        public IActionResult Ucak()
        {
            var fromCities = _context.Trips
               .Where(t => t.TransportType == "Uçak") // sadece otobüs seferleri
               .Select(t => t.FromCity)
               .Where(c => !string.IsNullOrWhiteSpace(c))
               .Distinct()
               .ToList(); // DB'den çekme

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Uçak") // sadece otobüs seferleri
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList(); // DB'den çekme

            // Birleştir ve tekrar distinct yapma
            var cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Hiç sefer yoksa fallback şehir listesi
            if (!cities.Any())
            {
                cities = new List<string> { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" };
            }

            ViewBag.Cities = cities;
            return View();
        }

        [HttpPost]
        public IActionResult Ucak(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            var fromCities = _context.Trips
                .Where(t => t.TransportType == "Uçak")
                .Select(t => t.FromCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Uçak")
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            ViewBag.Cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Arama sonucu
            var trips = _context.Trips
                .Where(t =>
                    t.FromCity == Nereden &&
                    t.ToCity == Nereye &&
                    t.DepartureDate.Date == GidisTarihi.Date)
                .ToList();


            return RedirectToAction("UcakListesi", new { from = Nereden, to = Nereye, tarih = GidisTarihi.ToString("dd.MM.yyyy") });
        }

        public IActionResult UcakListesi(string from, string to, string tarih)
        {
            UcakListesiVM _vm = new UcakListesiVM();

            //List<Trip> trips = new List<Trip>();
            //if (TempData["Trips"] != null)
            //{
            //    _vm.Trips= JsonSerializer.Deserialize<List<Trip>>(TempData["Trips"].ToString());
            //}

            #region alternatif yol
            List<Trip> trips = new List<Trip>();
            if (from != null && to != null && tarih != null)
            {
                DateTime dt = Convert.ToDateTime(tarih);

                // Arama sonucu
                var tripsResults = _context.Trips
                    .Where(t =>
                        t.FromCity == from &&
                        t.ToCity == to &&
                        t.DepartureDate.Date == dt)
                    .ToList();
                _vm.Trips = tripsResults;
                #endregion

                _vm.Nereden = from;
                _vm.Nereye = to;
                _vm.GidisTarihi = dt;


                return View(_vm);
            }
            else return View();
        }

        [HttpGet]
        public IActionResult Tren()
        {
            var fromCities = _context.Trips
                .Where(t => t.TransportType == "Tren") // sadece otobüs seferleri
                .Select(t => t.FromCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList(); // DB'den çekme

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Tren") // sadece otobüs seferleri
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList(); // DB'den çekme

            // Birleştir ve tekrar distinct yapma
            var cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Hiç sefer yoksa fallback şehir listesi
            if (!cities.Any())
            {
                cities = new List<string> { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" };
            }

            ViewBag.Cities = cities;
            return View();
        }

        [HttpPost]
        public IActionResult Tren(string Nereden, string Nereye, DateTime GidisTarihi, string YolculukTuru)
        {
            var fromCities = _context.Trips
                .Where(t => t.TransportType == "Tren")
                .Select(t => t.FromCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            var toCities = _context.Trips
                .Where(t => t.TransportType == "Tren")
                .Select(t => t.ToCity)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            ViewBag.Cities = fromCities
                .Concat(toCities)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Arama sonucu
            var trips = _context.Trips
                .Where(t =>
                    t.FromCity == Nereden &&
                    t.ToCity == Nereye &&
                    t.DepartureDate.Date == GidisTarihi.Date)
                .ToList();


            return RedirectToAction("TrenListesi", new { from = Nereden, to = Nereye, tarih = GidisTarihi.ToString("dd.MM.yyyy") });
        }
        public IActionResult TrenListesi(string from, string to, string tarih)
        {
            TrenListesiVM _vm = new TrenListesiVM();

            //List<Trip> trips = new List<Trip>();
            //if (TempData["Trips"] != null)
            //{
            //    _vm.Trips= JsonSerializer.Deserialize<List<Trip>>(TempData["Trips"].ToString());
            //}

            #region alternatif yol
            List<Trip> trips = new List<Trip>();
            if (from != null && to != null && tarih != null)
            {
                DateTime dt = Convert.ToDateTime(tarih);

                // Arama sonucu
                var tripsResults = _context.Trips
                    .Where(t =>
                        t.FromCity == from &&
                        t.ToCity == to &&
                        t.DepartureDate.Date == dt)
                    .ToList();
                _vm.Trips = tripsResults;
                #endregion

                _vm.Nereden = from;
                _vm.Nereye = to;
                _vm.GidisTarihi = dt;


                return View(_vm);
            }
            else return View();

        }

    }
}
