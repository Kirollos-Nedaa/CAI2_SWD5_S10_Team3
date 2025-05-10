using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class DiscountController : Controller
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return View(discounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            return View(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Discount discount)
        {
            await _discountService.AddDiscountAsync(discount);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Discount discount)
        {
            await _discountService.UpdateDiscountAsync(discount);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteDiscountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}