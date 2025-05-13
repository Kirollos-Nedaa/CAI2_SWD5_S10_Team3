// TechXpress.Web/Components/CartCountViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;

namespace TechXpress.Web.Components
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly CartServices _cartService;

        public CartCountViewComponent(CartServices cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = await _cartService.GetCartCountAsync();
            return View(count);
        }
    }
}