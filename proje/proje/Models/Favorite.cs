namespace IkinciElEsya.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Beğenen Kullanıcı
        public int ProductId { get; set; } // Beğenilen Ürün
    }
}