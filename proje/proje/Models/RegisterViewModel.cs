using System.ComponentModel.DataAnnotations;

namespace IkinciElEsya.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        // Ödevde Ad Soyad opsiyonel ama ApplicationUser'a eklediğimiz için alabiliriz
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}