using IkinciElEsya.Models;

namespace IkinciElEsya.Repositories.Abstract
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts(); // Tüm ürünleri getir
        Product GetProductById(int id); // Tek bir ürünü getir
        void AddProduct(Product product); // Ürün ekle
        void UpdateProduct(Product product); // Güncelle
        void DeleteProduct(int id); // Sil
        List<Product> GetProductsByCategoryId(int categoryId);
        List<Product> GetProductsByUserId(string userId);
    }
}