using Microsoft.AspNetCore.Identity;

namespace IkinciElEsya.Models
{
    // IdentityUser'dan miras alıyoruz, böylece şifre, email vb. hazır geliyor.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // YENİ EKSTRA BİLGİ:
        public string? City { get; set; }
    }
}