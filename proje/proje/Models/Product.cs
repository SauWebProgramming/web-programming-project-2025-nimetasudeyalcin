using IkinciElEsya.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkinciElEsya.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün başlığı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Ürün Başlığı")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Ürün Açıklaması")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Display(Name = "Fiyat")]
        [Column(TypeName = "decimal(18,2)")] // Para birimi için hassas ayar
        public decimal Price { get; set; }

        [Display(Name = "Ürün Görseli")]
        public string? ImageUrl { get; set; } // Resmin dosya yolunu tutacağız (Örn: /img/laptop.jpg)

        // Veritabanına otomatik tarih atacağız
        [Display(Name = "Eklenme Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // --- İLİŞKİLER ---

        // 1. Kategori İlişkisi (Bir ürünün bir kategorisi olur)
        [Required]
        public int CategoryId { get; set; } // Yabancı Anahtar (Foreign Key)

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // 2. Kullanıcı İlişkisi (Bir ürünü bir kullanıcı satar)
        // IdentityUser Id'si string (GUID) olarak tutulur.
        public string? SellerId { get; set; }
        // Not: Identity User sınıfını henüz bağlamadık, onu sonraki adımda ApplicationUser oluşturunca bağlayacağız.
    }
}