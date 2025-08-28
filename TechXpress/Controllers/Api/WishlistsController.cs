using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure;

namespace TechXpress.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WishlistsController : ControllerBase
    {
        private readonly WishlistService _wishlistService;
        private readonly ProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public WishlistsController(
            WishlistService wishlistService,
            ProductService productService,
            IUnitOfWork unitOfWork)
        {
            _wishlistService = wishlistService;
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
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
                        ImageUrl = product.ImageUrl,
                        Stock = product.Quantity
                    });
                }
            }

            return Ok(viewModel);
        }

        [HttpPost("toggle/{productId}")]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistRepository = _unitOfWork.GetRepository<Wishlist>();

            var allItems = await wishlistRepository.GetAllAsync();
            var existingItem = allItems
                .FirstOrDefault(w => w.Customer_Id == userId && w.Product_Id == productId);

            if (existingItem != null)
            {
                await wishlistRepository.DeleteAsync(existingItem);
                await _unitOfWork.CommitAsync();
                return Ok(new { message = "Removed from wishlist" });
            }
            else
            {
                await wishlistRepository.AddAsync(new Wishlist
                {
                    Customer_Id = userId,
                    Product_Id = productId
                });
                await _unitOfWork.CommitAsync();
                return Ok(new { message = "Added to wishlist" });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishlist()
        {
            await _wishlistService.ClearWishlistAsync();
            return Ok(new { message = "Wishlist cleared" });
        }
    }
}
