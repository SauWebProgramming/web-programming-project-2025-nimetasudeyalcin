using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IkinciElEsya.Data;
using IkinciElEsya.Models;

namespace IkinciElEsya.Controllers
{
    [Authorize] // Sadece üye olanlar beğenebilir
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var existingFav = _context.Favorites.FirstOrDefault(f => f.UserId == user.Id && f.ProductId == productId);

            bool isFavorited; // Sonuç ne oldu? (Beğendi mi, Vaz mı geçti?)

            if (existingFav != null)
            {
                _context.Favorites.Remove(existingFav); // Zaten favoriyse kaldır
                isFavorited = false;
            }
            else
            {
                _context.Favorites.Add(new Favorite { UserId = user.Id, ProductId = productId }); // Değilse ekle
                isFavorited = true;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, isFavorited = isFavorited });
        }
    }
}