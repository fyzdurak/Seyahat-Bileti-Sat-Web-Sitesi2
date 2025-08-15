using BiletSatisWebApp.Data;
using BiletSatisWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BiletSatisWebApp.Controllers
{
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var trips = _context.Trips.ToList();
            return View(trips);
        }
        //public IActionResult AdminTripList()
        //{
        //    DateTime now = DateTime.Now;

        //    var upcomingTrips = _context.Trips
        //        .Where(t => t.DepartureDate.Add(t.DepartureTime) >= now) // tarih + saat
        //        .OrderBy(t => t.DepartureDate)
        //        .ThenBy(t => t.DepartureTime)
        //        .ToList();

        //    var pastTrips = _context.Trips
        //        .Where(t => t.DepartureDate.Add(t.DepartureTime) < now) // geçmişse
        //        .OrderByDescending(t => t.DepartureDate)
        //        .ThenByDescending(t => t.DepartureTime)
        //        .ToList();

        //    var vm = new AdminTripListVM
        //    {
        //        UpcomingTrips = upcomingTrips,
        //        PastTrips = pastTrips
        //    };

        //    return View(vm);
        //}
        public IActionResult Buy(int id)
        {
            var trip = _context.Trips.Find(id);
            if (trip == null) return NotFound();

            // Burada satın alma işlemi yapılacak (Sepet, Order tablosu vs.)
            ViewBag.Message = "Bilet başarıyla satın alındı!";
            return RedirectToAction("Index");
        }
    }
}
