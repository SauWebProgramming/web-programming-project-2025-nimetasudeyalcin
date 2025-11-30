using System.ComponentModel.DataAnnotations;

namespace IkinciElEsya.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Email (Değiştirilemez)")]
        public string Email { get; set; }
    }
}