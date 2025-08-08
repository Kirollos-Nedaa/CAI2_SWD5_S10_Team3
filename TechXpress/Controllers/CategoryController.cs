using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure.Contexts;

namespace TechXpress.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryServices;
        private readonly S3Service _s3Service;
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryServices, S3Service s3Service, AppDbContext dbContext, ILogger<CategoryController> logger)
        {
            _categoryServices = categoryServices;
            _s3Service = s3Service;
            _context = dbContext;
            _logger = logger;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imageUrl = await _s3Service.UploadFileAsync(imageFile, "categories");
                        category.Img = imageUrl;
                    }

                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile imageFile)
        {
            if (id != category.Category_Id) return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imageUrl = await _s3Service.UploadFileAsync(imageFile, "category");
                        category.Img = imageUrl;
                    }

                    await _categoryServices.UpdateCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category {id}");
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryServices.DeleteCategoryAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting Category {id}");
                return View("Error");
            }
        }
    }
}
