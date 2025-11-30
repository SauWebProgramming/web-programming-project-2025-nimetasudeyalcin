using IkinciElEsya.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IkinciElEsya.Models; // Models klasörünü import etmeyi unutma

namespace IkinciElEsya.Data // Namespace'e dikkat et
{
    // IdentityDbContext'ten miras alıyoruz ve içine kendi User sınıfımızı veriyoruz.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanında oluşacak tabloları buraya yazıyoruz
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        // Veritabanı oluşurken çalışacak ayarlar (Opsiyonel ama önerilir)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Identity tabloları için gerekli ayarları yükler.
            // Buraya ekstra ilişki ayarları yazılabilir.
        }
    }
}