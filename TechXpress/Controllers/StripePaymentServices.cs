using Stripe;
using Stripe.Checkout;
using TechXpress.Domain.Models;

namespace TechXpress.Core.Services
{
    public class StripePaymentServices
    {
        private readonly IConfiguration _config;
        private readonly CartServices _cartService;

        public StripePaymentServices(IConfiguration config, CartServices cartService)
        {
            _config = config;
            _cartService = cartService;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<Session> CreateCheckoutSession(string userId, HttpRequest request)
        {
            var cart = await _cartService.GetCartAsync();
            var domain = $"{_config["AppSettings:BaseUrl"] ?? $"{request.Scheme}://{request.Host}"}";

            // Debug: Print all image paths
            Console.WriteLine("--- Product Image URLs ---");
            foreach (var item in cart.CartItems)
            {
                Console.WriteLine($"Product: {item.Product.Name}, Image: {item.Product.ImageUrl ?? "(null)"}");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.CartItems.Select(item =>
                {
                    var productData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name
                    };

                    // Debug: Print current item's image URL
                    Console.WriteLine($"Processing: {item.Product.Name}, Image: {item.Product.ImageUrl ?? "(null)"}");

                    // Only add Images if URL is valid
                    if (!string.IsNullOrEmpty(item.Product.ImageUrl))
                    {
                        productData.Images = new List<string> { item.Product.ImageUrl };
                    }

                    return new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100),
                            Currency = "usd",
                            ProductData = productData
                        },
                        Quantity = item.Quantity,
                    };
                }).ToList(),
                Mode = "payment",
                SuccessUrl = domain + "/Order/Confirm?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Cart",
                Metadata = new Dictionary<string, string> { { "user_id", userId } }
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }

        public async Task<bool> VerifyPayment(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            return session.PaymentStatus == "paid";
        }
    }
}