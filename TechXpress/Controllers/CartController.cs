using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class CartController : Controller
    {
        private IConfiguration _config;
        private readonly CartServices _cartService;
        private readonly ProductService _productService;
        private readonly StripePaymentServices _stripeService;

        public CartController(CartServices cartService, ProductService productService, StripePaymentServices stripeService)
        {
            _cartService = cartService;
            _productService = productService;
            _stripeService = stripeService;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = await _stripeService.CreateCheckoutSessionAsync(userId, Request);
            return Redirect(session.Url);
        }
    }
}