using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using IkinciElEsya.Models;
using IkinciElEsya.Repositories.Abstract;
using IkinciElEsya.Repositories.Concrete;
using Microsoft.AspNetCore.Hosting;

namespace IkinciElEsya.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment; // Resim kaydetmek için

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. LİSTELEME
        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        // 2. EKLEME SAYFASI
        public IActionResult Create()
        {
            // Kategorileri Dropdown (Açılır Kutu) için hazırlıyoruz
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View();
        }

        // 3. EKLEME İŞLEMİ (RESİM YÜKLEME DAHİL)
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string resimAdi = "no-image.png"; // Varsayılan resim

                // Eğer resim yüklendiyse
                if (model.ImageFile != null)
                {
                    string klasorYolu = Path.Combine(_webHostEnvironment.WebRootPath, "img");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(klasorYolu))
                        Directory.CreateDirectory(klasorYolu);

                    // Resme benzersiz bir isim ver (Örn: buzdolabi_GUID.jpg)
                    resimAdi = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string tamYol = Path.Combine(klasorYolu, resimAdi);

                    // Resmi sunucuya kaydet
                    using (var stream = new FileStream(tamYol, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }

                // ViewModel'i Gerçek Model'e (Entity) çevir
                Product product = new Product
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    ImageUrl = "/img/" + resimAdi, // Veritabanına yolunu yazıyoruz
                    CreatedDate = DateTime.Now
                    // SellerId = ... (Giriş yapma kısmını yapınca burayı dolduracağız)
                };

                _productRepository.AddProduct(product);
                return RedirectToAction("Index");
            }

            // Hata varsa kategorileri tekrar yükle
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View(model);
        }
        // SİLME İŞLEMİ
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetProductById(id);

            if (product != null)
            {
                // İstersen burada klasördeki resmi silme kodu da yazılabilir ama şart değil.
                _productRepository.DeleteProduct(id);
            }

            return RedirectToAction("Index");
        }
        // 4. DÜZENLEME SAYFASI (Verileri Getirir)
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            // Veritabanındaki ürünü (Entity), ekrandaki modele (ViewModel) çeviriyoruz
            var model = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageUrl // Eski resmi göstermek için
            };

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(model);
        }

        // 5. DÜZENLEME İŞLEMİ (Verileri Günceller)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.GetProductById(model.Id);
                if (product == null) return NotFound();

                // Bilgileri güncelle
                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                // Eğer YENİ RESİM seçildiyse onu kaydet
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
                // Yeni resim seçilmediyse product.ImageUrl'e dokunmuyoruz, eskisi kalıyor.

                _productRepository.UpdateProduct(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", model.CategoryId);
            return View(model);
        }
    }
}