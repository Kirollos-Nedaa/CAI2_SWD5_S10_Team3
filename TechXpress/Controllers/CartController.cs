using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly CartServices _cartService;
        private readonly ProductService _productService;

        public CartController(
    CartServices cartService,
    ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            var viewModel = new CartViewModel
            {
                Items = new List<CartItemViewModel>()
            };

            foreach (var item in cart.CartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.Product_Id);
                viewModel.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Product_Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = item.Quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            viewModel.Total = viewModel.Items.Sum(i => i.Price * i.Quantity);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _cartService.RemoveFromCartAsync(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(productId, quantity);
            return RedirectToAction("Index");
        }


















    }
}