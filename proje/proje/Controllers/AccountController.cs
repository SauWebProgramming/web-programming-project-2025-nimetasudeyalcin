using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IkinciElEsya.Models;
using Microsoft.AspNetCore.Authorization;

namespace IkinciElEsya.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Identity servislerini buraya çağırıyoruz
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 1. KAYIT OLMA SAYFASI (Register)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    // YENİ ALANLARI EŞLEŞTİRİYORUZ:
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kayıt başarılıysa otomatik giriş yap ve anasayfaya yönlendir
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Hata varsa (örn: şifre çok basitse) hataları ekrana bas
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // 2. GİRİŞ YAPMA SAYFASI (Login)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Geçersiz email veya şifre.");
            }
            return View(model);
        }

        // 3. ÇIKIŞ YAPMA (Logout)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // 1. PROFİL SAYFASINI GETİR
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Email = user.Email
            };

            return View(model);
        }

        // 2. PROFİLİ GÜNCELLE (POST)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.City = model.City;
                // Email değiştirmeyi güvenlik gereği şimdilik kapalı tutuyoruz

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
                    return RedirectToAction("Profile"); // Sayfayı yenile
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}