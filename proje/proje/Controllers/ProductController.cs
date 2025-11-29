using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Kullanıcıyı bulmak için lazım
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IkinciElEsya.Models;
using IkinciElEsya.Repositories.Abstract;

namespace IkinciElEsya.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager; // EKLENDİ

        public ProductController(IProductRepository productRepository,
                                 ICategoryRepository categoryRepository,
                                 IWebHostEnvironment webHostEnvironment,
                                 UserManager<ApplicationUser> userManager) // EKLENDİ
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // 1. LİSTELEME (HERKES GÖREBİLİR)
        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        // 2. EKLEME SAYFASI
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View();
        }

        // 3. EKLEME İŞLEMİ
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string resimAdi = "no-image.png";
                if (model.ImageFile != null)
                {
                    string klasorYolu = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    if (!Directory.Exists(klasorYolu)) Directory.CreateDirectory(klasorYolu);
                    resimAdi = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    using (var stream = new FileStream(Path.Combine(klasorYolu, resimAdi), FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }

                // EKLENDİ: Şu an giriş yapmış kullanıcının ID'sini alıyoruz
                var userId = _userManager.GetUserId(User);

                Product product = new Product
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    ImageUrl = "/img/" + resimAdi,
                    CreatedDate = DateTime.Now,
                    SellerId = userId // ARTIK SATICI BELLİ!
                };

                _productRepository.AddProduct(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View(model);
        }

        // 4. DÜZENLEME SAYFASI
        [Authorize]
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            // GÜVENLİK KONTROLÜ: Ürün benim değilse VE Admin değilsem düzenleyemem!
            var currentUserId = _userManager.GetUserId(User);
            if (product.SellerId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403 Erişim Yasak Hatası ver
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageUrl
            };

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(model);
        }

        // 5. DÜZENLEME İŞLEMİ
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var product = _productRepository.GetProductById(model.Id);
            if (product == null) return NotFound();

            // GÜVENLİK KONTROLÜ (POST işlemi için de şart)
            var currentUserId = _userManager.GetUserId(User);
            if (product.SellerId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                if (model.ImageFile != null)
                {
                    string klasorYolu = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    string resimAdi = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    using (var stream = new FileStream(Path.Combine(klasorYolu, resimAdi), FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/img/" + resimAdi;
                }

                _productRepository.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        // 6. SİLME İŞLEMİ
        [Authorize]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product != null)
            {
                // GÜVENLİK KONTROLÜ: Sadece sahibi veya Admin silebilir
                var currentUserId = _userManager.GetUserId(User);
                if (product.SellerId == currentUserId || User.IsInRole("Admin"))
                {
                    _productRepository.DeleteProduct(id);
                }
            }
            return RedirectToAction("Index");
        }
    }
}