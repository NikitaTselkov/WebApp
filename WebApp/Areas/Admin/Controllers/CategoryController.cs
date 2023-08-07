using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _categoryRepo.GetAll();
            return View(objCategoryList);
        }

        // Get
        public IActionResult Create()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // Get
        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var categoryFromDB = _categoryRepo.Get(g => g.Id == id);

            if (categoryFromDB is null)
                return NotFound();

            return View(categoryFromDB);
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(category);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // Get
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var categoryFromDB = _categoryRepo.Get(g => g.Id == id);

            if (categoryFromDB is null)
                return NotFound();

            return View(categoryFromDB);
        }

        // Post
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var categoryFromDB = _categoryRepo.Get(g => g.Id == id);

            if (categoryFromDB is null)
                return NotFound();

            _categoryRepo.Remove(categoryFromDB);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
