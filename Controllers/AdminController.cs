using BiletSatisWebApp.Data;
using BiletSatisWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BiletSatisWebApp.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin rolü olanlar erişebilir
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // İstatistik kartları için admin anasayfa
        public async Task<IActionResult> Index()
        {
            var totalTicketsSold = await _context.Trips.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var todaySales = await _context.Trips
                .Where(t => t.DepartureDate.Date == DateTime.Today)
                  .SumAsync(t => t.TicketsSold);

            ViewBag.TotalTicketsSold = totalTicketsSold;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TodaySales = todaySales;

            return View();
        }

        // Bilet satışa çıkarma sayfası (GET)
        [HttpGet]
        public IActionResult SellTicket()
        {
            return View();
        }

        // Bilet satışa çıkarma işlemi (POST)
        [HttpPost]
        public IActionResult SellTicket(Trip model)
        {
            if (model.DepartureDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("DepartureDate", "Bilet kalkış tarihi bugünden önce olamaz.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                model.SaleDate = DateTime.Now;
                model.TicketsSold = 0; // Başlangıçta hiç satılmadı
                _context.Trips.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Bilet başarıyla satışa çıkarıldı.";
                return RedirectToAction("SellTicket");
            }
            return View(model);
        }

        // Mevcut seyahat işlemleri:

        public IActionResult Trips()
        {
            var now = DateTime.Now;

            var trips = _context.Trips.ToList(); // Verileri belleğe alıyoruz

            var model = new AdminTripListVM
            {
                UpcomingTrips = trips
                    .Where(t => t.DepartureDate.Add(t.DepartureTime) >= now)
                    .OrderBy(t => t.DepartureDate)
                    .ThenBy(t => t.DepartureTime)
                    .ToList(),

                PastTrips = trips
                    .Where(t => t.DepartureDate.Add(t.DepartureTime) < now)
                    .OrderByDescending(t => t.DepartureDate)
                    .ThenByDescending(t => t.DepartureTime)
                    .ToList()
            };

            return View(model);
        }



        public IActionResult DeleteTrip(int id)
        {
            var trip = _context.Trips.Find(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                _context.SaveChanges();
            }
            return RedirectToAction("Trips");
        }
    }
}
