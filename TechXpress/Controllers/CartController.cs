using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly CartServices _cartService;

        public CartController(CartServices cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index() => View(await _cartService.GetAllCartsAsync());
        public async Task<IActionResult> Details(int id) => View(await _cartService.GetCartByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Cart cart)
        {
            await _cartService.AddCartAsync(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Cart cart)
        {
            await _cartService.UpdateCartAsync(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartService.DeleteCartAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}