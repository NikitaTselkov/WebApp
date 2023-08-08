using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Utility;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _productRepo.GetAll(includeProperties:"Category").ToList();

            return View(objList);
        }

        // Get
        public IActionResult Upsert(int? id)
        {
            ViewData["CategoryList"] = _categoryRepo.GetAll().Select(s => new SelectListItem {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            //ViewBag.CategoryList = categoryList;

            if (id is null || id == 0)
                return View(new Product());
            else
                return View(_productRepo.Get(g => g.Id == id));
            
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product, IFormFile? file)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file is not null)
            { 
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

                    if(System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                 product.ImageUrl = @"\images\product\" + fileName;
            }

            if (ModelState.IsValid)
            {
                var tmpText = string.Empty;

                if (product.Id == 0)
                {
                    _productRepo.Add(product);
                    tmpText = "created";
                }
                else
                {
                    _productRepo.Update(product);
                    tmpText = "updated";
                }

                _productRepo.Save();
                TempData["success"] = $"Product {tmpText} successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["CategoryList"] = _categoryRepo.GetAll().Select(s => new SelectListItem {
                    Text = s.Name,
                    Value = s.Id.ToString()
                }); 
            }

            return View(product);
        }

        // Get
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var productFromDB = _productRepo.Get(g => g.Id == id);

            if (productFromDB is null)
                return NotFound();

            return View(productFromDB);
        }

        // Post
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var productFromDB = _productRepo.Get(g => g.Id == id);

            if (productFromDB is null)
                return NotFound();

            _productRepo.Remove(productFromDB);
            _productRepo.Save();
            TempData["success"] = "Product deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
