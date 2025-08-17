using BiletSatisWebApp.Data;
using BiletSatisWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace BiletSatisWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hakkimizda()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Iletisim(IletisimModel model)
        {
            if (ModelState.IsValid)
            {
                var mesaj = new IletisimMesaj
                {
                    AdSoyad = model.AdSoyad,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    Mesaj = model.Mesaj
                };

                _context.IletisimMesajlar.Add(mesaj);
                _context.SaveChanges();

                TempData["ShowModal"] = "true"; // Baþarý mesajý için
                return RedirectToAction("GelenKutusu", "Admin");
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
