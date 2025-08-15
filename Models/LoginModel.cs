using System.ComponentModel.DataAnnotations;

namespace BiletSatisWebApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz.")]
        public string Password { get; set; }

        public string UserType { get; set; } // User veya Admin

        public bool RememberMe { get; set; }
    }
}
