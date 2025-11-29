using System.ComponentModel.DataAnnotations;

namespace IkinciElEsya.Models // veya ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [Display(Name = "Ürün Adı")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Display(Name = "Fiyat (₺)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Lütfen bir kategori seçin.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        // DİKKAT: Resim dosyası burada taşınacak
        [Display(Name = "Ürün Görseli")]
        public IFormFile? ImageFile { get; set; }

        // Düzenleme yaparken eski resmi göstermek için
        public string? ExistingImageUrl { get; set; }
    }
}