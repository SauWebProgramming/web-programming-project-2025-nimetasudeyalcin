using Microsoft.AspNetCore.Identity;

namespace IkinciElEsya.Models
{
    // IdentityUser'dan miras alıyoruz, böylece şifre, email vb. hazır geliyor.
    public class ApplicationUser : IdentityUser
    {
        // Eklemek istediğin ekstra özellikler varsa buraya yazabilirsin.
        // Ödevde Ad Soyad zorunlu değil ama istersen ekle:
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}