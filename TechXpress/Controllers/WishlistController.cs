using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly WishlistService _wishlistService;

        public WishlistController(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            var wishlists = await _wishlistService.GetAllWishlistsAsync();
            return View(wishlists);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Wishlist wishlist)
        {
            await _wishlistService.AddWishlistAsync(wishlist);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Wishlist wishlist)
        {
            await _wishlistService.UpdateWishlistAsync(wishlist);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _wishlistService.DeleteWishlistAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}