using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IkinciElEsya.Data;
using IkinciElEsya.Models;
using IkinciElEsya.Repositories.Abstract; // Yerleri burasý (En tepe)
using IkinciElEsya.Repositories.Concrete; // Yerleri burasý (En tepe)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ---------------- VERÝTABANI VE IDENTITY AYARLARI ----------------

// 1. Veritabaný Baðlantýsý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (Üyelik) Ayarlarý
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. REPOSITORY DEPENDENCY INJECTION (DI) TANIMLAMALARI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// -----------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication (Kimlik Doðrulama) sýrasý çok önemli!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// --- SEED DATA (OTOMATÝK ADMIN EKLEME) ---
// Uygulama ayaða kalkarken bir scope oluþturup SeedData'yý çalýþtýrýyoruz
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // SeedData sýnýfýndaki Initialize metodunu çaðýr
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanýna seed data eklenirken bir hata oluþtu.");
    }
}
// -----------------------------------------
app.Run();