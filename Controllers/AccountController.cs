using BiletSatis.Models;
using BiletSatisWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BiletSatisWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ------------------- KAYIT -------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Ad = model.FirstName, // Burada FirstName'i AspNetUsers.Ad sütununa yazıyoruz
                    Soyad = model.LastName,
                    UserType = "User"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        

        // ------------------- GİRİŞ -------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz giriş denemesi.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Eğer kullanıcı Admin rolündeyse admin paneline yönlendir
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                // Normal kullanıcı ise anasayfaya
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
            return View(model);
        }


        // ------------------- ÇIKIŞ -------------------
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // -------------- PROFİL -----------------------
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Eğer seyahat verilerini tutan bir tablon varsa buradan çekersin
            // Örneğin:
            // var trips = await _context.Trips
            //     .Where(t => t.UserId == user.Id)
            //     .ToListAsync();

            var model = new ProfileViewModel
            {
                Ad = user.Ad,
                Soyad = user.Soyad,
                Email = user.Email,
                UserType = user.UserType,
                
            };

            return View(model);
        }

    }
}



