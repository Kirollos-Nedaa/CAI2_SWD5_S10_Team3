using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductServices _productService;
        private readonly CategoryServices _categoryServices;
        private readonly BrandServices _brandServices;

        public ProductController(ProductServices productService, CategoryServices categoryServices, BrandServices brandServices)
        {
            _productService = productService;
            _categoryServices = categoryServices;
            _brandServices = brandServices;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryServices.GetAllCategoriesAsync();
            var brands = await _brandServices.GetAllBrandsAsync();

            var viewModel = new ProductViewModel
            {
                Products = products.ToList(),
                Categories = categories.ToList(),
                Brands = brands.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryServices.GetAllCategoriesAsync();
            ViewBag.Category_Id = new SelectList(categories, "Category_Id", "Name");
            var brands = await _brandServices.GetAllBrandsAsync();
            ViewBag.Brand_Id = new SelectList(brands, "Brand_Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            await _productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}