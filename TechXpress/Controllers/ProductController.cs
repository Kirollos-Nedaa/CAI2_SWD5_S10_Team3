using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure;

namespace TechXpress.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly BrandServices _brandService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ProductService productService,
            CategoryService categoryService,
            BrandServices brandService,
            IMapper mapper, 
            ILogger<ProductController> logger,
            IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(List<int> selectedBrands, List<int> selectedCategories, int? maxPrice)
        {
            try
            {
                // Get all base data
                var products = (await _productService.GetAllProductsAsync()).ToList();
                var categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                var brands = (await _brandService.GetAllBrandsAsync()).ToList();

                // Apply filters
                if (selectedBrands != null && selectedBrands.Any())
                {
                    products = products.Where(p => selectedBrands.Contains(p.Brand_Id)).ToList();
                }

                if (selectedCategories != null && selectedCategories.Any())
                {
                    products = products.Where(p => selectedCategories.Contains(p.Category_Id)).ToList();
                }

                if (maxPrice.HasValue)
                {
                    products = products.Where(p => p.Price <= maxPrice.Value).ToList();
                }

                // Create view model
                var viewModel = new ProductViewModel
                {
                    Products = products,
                    WishlistProductIds = await GetWishlistProductIds(products)
                };

                // Pass data to ViewBag (keeping your existing ViewBag usage)
                ViewBag.Products = products;
                ViewBag.Categories = categories;
                ViewBag.Brands = brands;
                ViewBag.SelectedBrands = selectedBrands ?? new List<int>();
                ViewBag.SelectedCategories = selectedCategories ?? new List<int>();
                ViewBag.MaxPrice = maxPrice;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                return View("Error");
            }
        }

        private async Task<HashSet<int>> GetWishlistProductIds(List<Product> products)
        {
            var wishlistProductIds = new HashSet<int>();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishlistRepository = _unitOfWork.GetRepository<Wishlist>();

                // Get all wishlist items for the user
                var allWishlistItems = await wishlistRepository.GetAllAsync();

                // Filter to only include products that exist in the current filtered view
                wishlistProductIds = new HashSet<int>(allWishlistItems
                    .Where(w => w.Customer_Id == userId && products.Any(p => p.Product_Id == w.Product_Id))
                    .Select(w => w.Product_Id));
            }

            return wishlistProductIds;
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