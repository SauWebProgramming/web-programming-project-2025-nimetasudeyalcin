using Microsoft.AspNetCore.Identity;
using IkinciElEsya.Models;

namespace IkinciElEsya.Data
{
    public static class SeedData
    {
        // Bu metot Program.cs'den çağrılacak
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. ROLLERİ OLUŞTUR (Eğer yoksa)
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Rol veritabanında var mı kontrol et
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // Yoksa oluştur
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. ADMIN KULLANICISINI OLUŞTUR (Eğer yoksa)
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Sistem",
                    LastName = "Yöneticisi",
                    EmailConfirmed = true
                };

                // Admin kullanıcısını oluştur (Şifre: Admin123!)
                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    // Kullanıcıya "Admin" rolünü ata
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}