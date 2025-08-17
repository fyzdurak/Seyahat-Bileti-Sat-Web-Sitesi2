using System;
using System.ComponentModel.DataAnnotations;

namespace BiletSatisWebApp.Models
{
    public class IletisimMesaj
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string AdSoyad { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [Required]
        public string Mesaj { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
