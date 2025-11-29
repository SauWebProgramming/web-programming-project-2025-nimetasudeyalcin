using System.ComponentModel.DataAnnotations;

namespace IkinciElEsya.Models // ProjeAdin kısmını kendi proje adınla değiştir
{
    public class Category
    {
        // Birincil Anahtar (Primary Key)
        public int Id { get; set; }

        // Zorunlu alan ve max 50 karakter (Validation kuralı)
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; } // Soru işareti (?) boş geçilebilir demek.

        // İlişki: Bir kategoride birden çok ürün olabilir (One-to-Many)
        public virtual ICollection<Product> Products { get; set; }
    }
}