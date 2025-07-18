using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private const string ProductsImageFolder = "products";
        private const string CategoriesImageFolder = "categories";
        private const string BrandsImageFolder = "brands";

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly BrandServices _brandService;
        private readonly IMapper _mapper;
        private readonly S3Service _s3Service;

        public AdminController(
            ProductService productService,
            CategoryService categoryService,
            BrandServices brandService,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            S3Service s3Service)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _s3Service = s3Service;
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> Products() =>
            View(await _productService.GetAllProductsAsync());

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(productDto);
            }

            // Upload to S3
            if (productDto.Image != null && productDto.Image.Length > 0)
            {
                var imageUrl = await _s3Service.UploadFileAsync(productDto.Image, "products");
                productDto.ImageUrl = imageUrl;
            }

            var product = _mapper.Map<Product>(productDto);

            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await GetProductOrNotFound(id);
            if (product == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(_mapper.Map<EditProductDto>(product));
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductDto editDto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(editDto);
            }

            var product = await GetProductOrNotFound(editDto.Product_Id);
            if (product == null) return NotFound();

            await UpdateProductImageAsync(editDto, product);
            _mapper.Map(editDto, product);

            await _productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Products));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> Categories() =>
            View(await _categoryService.GetAllCategoriesAsync());

        [HttpGet]
        public IActionResult CreateCategory() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return View(categoryDto);

            var category = _mapper.Map<Category>(categoryDto);
            category.Img = await HandleImageUploadAsync(categoryDto.ImageFile, CategoriesImageFolder);

            await _categoryService.AddCategoryAsync(category);
            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await GetCategoryOrNotFound(id);
            return category == null ? NotFound() : View(_mapper.Map<EditCategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryDto editDto)
        {
            if (!ModelState.IsValid) return View(editDto);

            var category = await GetCategoryOrNotFound(editDto.Category_Id);
            if (category == null) return NotFound();

            await UpdateCategoryImageAsync(editDto, category);
            _mapper.Map(editDto, category);

            await _categoryService.UpdateCategoryAsync(category);
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Categories));
        }





        public async Task<IActionResult> Brands() =>
    View(await _brandService.GetAllBrandsAsync());



        [HttpGet]
        public IActionResult CreateBrand() => View();



        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandDto brandDto)
        {
            if (!ModelState.IsValid) return View(brandDto);

            var brand = _mapper.Map<Brand>(brandDto);
            brand.imgUrl = await HandleImageUploadAsync(brandDto.ImageFile, BrandsImageFolder);

            await _brandService.AddBrandAsync(brand);
            return RedirectToAction(nameof(Brands));
        }



        [HttpGet]
        public async Task<IActionResult> EditBrand(int id)
        {
            var brand = await GetBrandOrNotFound(id);
            return brand == null ? NotFound() : View(_mapper.Map<EditBrandDto>(brand));
        }


        [HttpPost]
        public async Task<IActionResult> EditBrand(EditBrandDto editDto)
        {
            if (!ModelState.IsValid) return View(editDto);

            var brand = await GetBrandOrNotFound(editDto.Brand_Id);
            if (brand == null) return NotFound();

            // Preserve existing image if no new file is uploaded
            if (editDto.ImageFile == null || editDto.ImageFile.Length == 0)
            {
                editDto.ImgUrl = brand.imgUrl;
            }

            // Map other properties first
            _mapper.Map(editDto, brand);

            // Handle image upload only if new file is provided
            if (editDto.ImageFile != null && editDto.ImageFile.Length > 0)
            {
                await DeleteOldImageAsync(brand.imgUrl);
                brand.imgUrl = await HandleImageUploadAsync(editDto.ImageFile, BrandsImageFolder);
            }

            await _brandService.UpdateBrandAsync(brand);
            return RedirectToAction(nameof(Brands));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction(nameof(Brands));
        }










        #region Private Helpers

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Categories = new SelectList(
                await _categoryService.GetAllCategoriesAsync(),
                "Category_Id",
                "Name");

            ViewBag.Brands = new SelectList(
                await _brandService.GetAllBrandsAsync(),
                "Brand_Id",
                "Name");
        }

        private async Task<string> HandleImageUploadAsync(IFormFile imageFile, string folderName)
        {
            if (imageFile == null || imageFile.Length == 0)
                return string.Empty;

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName);
            Directory.CreateDirectory(uploadsFolder);

            // Replace spaces with underscores in the filename
            var sanitizedFileName = Path.GetFileName(imageFile.FileName).Replace(" ", "_");
            var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}"; // Example: abc123_Screenshot_2025.png

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return uniqueFileName; // Return filename only (no path)
        }

        private async Task UpdateProductImageAsync(EditProductDto dto, Product product)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0) return;

            await DeleteOldImageAsync(product.ImageUrl);
            product.ImageUrl = await HandleImageUploadAsync(dto.ImageFile, ProductsImageFolder);
        }

        private async Task UpdateCategoryImageAsync(EditCategoryDto dto, Category category)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0) return;

            await DeleteOldImageAsync(category.Img);
            category.Img = await HandleImageUploadAsync(dto.ImageFile, CategoriesImageFolder);
        }

        private async Task DeleteOldImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                await Task.Run(() => System.IO.File.Delete(oldImagePath));
            }
        }

        private async Task<Product> GetProductOrNotFound(int id) =>
            await _productService.GetProductByIdAsync(id);

        private async Task<Category> GetCategoryOrNotFound(int id) =>
            await _categoryService.GetCategoryByIdAsync(id);


        private async Task<Brand> GetBrandOrNotFound(int id) =>
    await _brandService.GetBrandByIdAsync(id);


        private async Task UpdateBrandImageAsync(EditBrandDto dto, Brand brand)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0) return;

            await DeleteOldImageAsync(brand.imgUrl);
            brand.imgUrl = await HandleImageUploadAsync(dto.ImageFile, BrandsImageFolder);
        }


        #endregion
    }
}