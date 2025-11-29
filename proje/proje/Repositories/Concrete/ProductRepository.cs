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
            // Include(x => x.Category) ÇOK ÖNEMLİ!
            // Bunu yapmazsan ürünler gelir ama hangi kategoride oldukları boş gelir.
            return _context.Products.Include(x => x.Category).Include(x => x.SellerId).ToList();
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