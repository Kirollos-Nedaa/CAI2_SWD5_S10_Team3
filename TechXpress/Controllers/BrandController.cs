using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly BrandServices _brandService;

        public BrandController(BrandServices brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index() => View(await _brandService.GetAllBrandsAsync());
        public async Task<IActionResult> Details(int id) => View(await _brandService.GetBrandByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            await _brandService.AddBrandAsync(brand);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Brand brand)
        {
            await _brandService.UpdateBrandAsync(brand);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}