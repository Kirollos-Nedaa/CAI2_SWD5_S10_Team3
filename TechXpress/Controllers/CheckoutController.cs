using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;

namespace TechXpress.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly StripePaymentServices _stripe;
        private readonly CartServices _cartService;

        public CheckoutController(StripePaymentServices stripe, CartServices cartService)
        {
            _stripe = stripe;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Review()
        {
            // We'll implement this together
            return View();
        }
    }
}
