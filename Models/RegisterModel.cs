using System.ComponentModel.DataAnnotations;

namespace BiletSatisWebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Adınızı giriniz.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadınızı giriniz.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre giriniz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
