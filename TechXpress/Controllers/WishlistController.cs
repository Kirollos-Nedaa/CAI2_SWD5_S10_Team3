using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure;

namespace TechXpress.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly WishlistService _wishlistService;
        private readonly ProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public WishlistController(WishlistService wishlistService, ProductService productService, IUnitOfWork unitOfWork)
        {
            _wishlistService = wishlistService;
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var productIds = await _wishlistService.GetWishlistAsync();
            var viewModel = new WishlistViewModel();

            foreach (var id in productIds)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    viewModel.Items.Add(new WishlistItemViewModel
                    {
                        ProductId = product.Product_Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl
                    });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            await _wishlistService.AddToWishlistAsync(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishlistRepository = _unitOfWork.GetRepository<Wishlist>();

                // Get all items first, then filter in memory
                var allItems = await wishlistRepository.GetAllAsync();
                var existingItem = allItems
                    .FirstOrDefault(w => w.Customer_Id == userId && w.Product_Id == productId);

                if (existingItem != null)
                {
                    await wishlistRepository.DeleteAsync(existingItem);
                }
                else
                {
                    await wishlistRepository.AddAsync(new Wishlist
                    {
                        Customer_Id = userId,
                        Product_Id = productId
                    });
                }

                await _unitOfWork.CommitAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            await _wishlistService.RemoveFromWishlistAsync(productId);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ClearWishlist()
        {
            await _wishlistService.ClearWishlistAsync();
            return RedirectToAction("Index");
        }
    }
}