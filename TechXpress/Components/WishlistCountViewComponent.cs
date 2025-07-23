// File: ViewComponents/WishlistCountViewComponent.cs

using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;

namespace TechXpress.Web.Components
{
    public class WishlistCountViewComponent : ViewComponent
    {
        private readonly WishlistService _wishlistService;

        public WishlistCountViewComponent(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = await _wishlistService.GetWishlistCountAsync();
            return View(count);
        }
    }
}
