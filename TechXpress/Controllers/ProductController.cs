using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly BrandServices _brandService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ProductService productService,
            CategoryService categoryService,
            BrandServices brandService,
            IMapper mapper, 
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(List<int> selectedBrands, List<int> selectedCategories, int? maxPrice)
        {
            try
            {
                var products = (await _productService.GetAllProductsAsync()).ToList();
                var categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                var brands = (await _brandService.GetAllBrandsAsync()).ToList();

                // Apply Brand Filter
                if (selectedBrands != null && selectedBrands.Any())
                {
                    products = products.Where(p => selectedBrands.Contains(p.Brand_Id)).ToList();
                }

                // Apply Category Filter
                if (selectedCategories != null && selectedCategories.Any())
                {
                    products = products.Where(p => selectedCategories.Contains(p.Category_Id)).ToList();
                }

                // Apply Max Price Filter
                if (maxPrice.HasValue)
                {
                    products = products.Where(p => p.Price <= maxPrice.Value).ToList();
                }

                // Pass data to ViewBag
                ViewBag.Products = products;
                ViewBag.Categories = categories;
                ViewBag.Brands = brands;

                // Pass selected filter values so checkboxes and slider retain values
                ViewBag.SelectedBrands = selectedBrands ?? new List<int>();
                ViewBag.SelectedCategories = selectedCategories ?? new List<int>();
                ViewBag.MaxPrice = maxPrice;

                var viewModel = new ProductViewModel
                {
                    Products = products
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id, includeRelated: true);
               var ProductDto = _mapper.Map<ProductDetailsDto>(product);


                return product == null ? NotFound() : View(ProductDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting product {id}");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.AddProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                await PopulateDropdowns(product.Category_Id, product.Brand_Id);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                await PopulateDropdowns();
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                await PopulateDropdowns(product.Category_Id, product.Brand_Id);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading edit form for product {id}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Product_Id) return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                await PopulateDropdowns(product.Category_Id, product.Brand_Id);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product {id}");
                await PopulateDropdowns(product.Category_Id, product.Brand_Id);
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                return View("Error");
            }
        }

        private async Task PopulateDropdowns(int? categoryId = null, int? brandId = null)
        {
            ViewBag.Category_Id = new SelectList(
                await _categoryService.GetAllCategoriesAsync(),
                "Category_Id",
                "Name",
                categoryId);

            ViewBag.Brand_Id = new SelectList(
                await _brandService.GetAllBrandsAsync(),
                "Brand_Id",
                "Name",
                brandId);
        }
    }
}