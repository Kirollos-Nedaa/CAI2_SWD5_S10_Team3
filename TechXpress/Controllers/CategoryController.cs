using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryServices;

        public CategoryController(CategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryServices.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _categoryServices.AddCategoryAsync(category);
            var categories = await _categoryServices.GetAllCategoriesAsync();
            return View(nameof(Index), categories);
        }
    }
}
