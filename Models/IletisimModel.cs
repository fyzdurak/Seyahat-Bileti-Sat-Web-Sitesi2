using System.ComponentModel.DataAnnotations;

namespace BiletSatisWebApp.Models
{
    public class IletisimModel
    {
        [Required(ErrorMessage = "Adınızı giriniz.")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "E-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numaranızı giriniz.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Mesajınızı yazınız.")]
        public string Mesaj { get; set; }
    }
}

