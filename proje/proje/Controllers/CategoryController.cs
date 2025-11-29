using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IkinciElEsya.Models;
using IkinciElEsya.Repositories.Abstract;

namespace IkinciElEsya.Controllers
{
    [Authorize(Roles = "Admin")] // <-- DİKKAT: Test etmek için şimdilik kapalı kalsın. Admin ekleyince açacağız.
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        // Dependency Injection ile Repository'i içeri alıyoruz
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // 1. LİSTELEME SAYFASI
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }

        // 2. EKLEME SAYFASI (Formu Gösterir)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. EKLEME İŞLEMİ (Formdan Gelen Veriyi Kaydeder)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.AddCategory(category);
                return RedirectToAction("Index"); // Listeye geri dön
            }
            return View(category); // Hata varsa formu tekrar göster
        }

        // 3. SİLME İŞLEMİ
        public IActionResult Delete(int id)
        {
            _categoryRepository.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}