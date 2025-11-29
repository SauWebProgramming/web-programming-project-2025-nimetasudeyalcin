using IkinciElEsya.Data;
using IkinciElEsya.Models;
using IkinciElEsya.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace IkinciElEsya.Repositories.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor Injection: Veritabanı bağlantısını buraya alıyoruz
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges(); // Kaydetmeyi unutma!
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<Product> GetAllProducts()
        {
            // SellerId zaten ürünün içinde bir sütun olduğu için otomatik gelir.
            // Sadece Category tablosunu Include etmemiz yeterli.
            return _context.Products.Include(x => x.Category).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}