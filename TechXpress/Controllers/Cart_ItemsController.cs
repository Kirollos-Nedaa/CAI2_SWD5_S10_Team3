using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class CartItemController : Controller
    {
        //private readonly Cart_ItemsServices _cartItemService;

        //public CartItemController(Cart_ItemsServices cartItemService)
        //{
        //    _cartItemService = cartItemService;
        //}

        //public async Task<IActionResult> Index() => View(await _cartItemService.GetAllCartItemsAsync());
        //public async Task<IActionResult> Details(int id) => View(await _cartItemService.GetCartItemByIdAsync(id));

        //[HttpPost]
        //public async Task<IActionResult> Create(Cart_Items cartItem)
        //{
        //    await _cartItemService.AddCartItemAsync(cartItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Cart_Items cartItem)
        //{
        //    await _cartItemService.UpdateCartItemAsync(cartItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _cartItemService.DeleteCartItemAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}