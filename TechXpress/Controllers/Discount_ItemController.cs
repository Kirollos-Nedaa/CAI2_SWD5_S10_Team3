using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class DiscountItemController : Controller
    {
        //private readonly DiscountItemService _discountItemService;

        //public DiscountItemController(DiscountItemService discountItemService)
        //{
        //    _discountItemService = discountItemService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var discountItems = await _discountItemService.GetAllDiscountItemsAsync();
        //    return View(discountItems);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    var discountItem = await _discountItemService.GetDiscountItemByIdAsync(id);
        //    return View(discountItem);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Discount_Item discountItem)
        //{
        //    await _discountItemService.AddDiscountItemAsync(discountItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Discount_Item discountItem)
        //{
        //    await _discountItemService.UpdateDiscountItemAsync(discountItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _discountItemService.DeleteDiscountItemAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}