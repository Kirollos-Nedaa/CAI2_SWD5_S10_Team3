using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TechXpress.Core.Services
{
    public class StripePaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _stripeSecretKey;
        private readonly CartServices _cartService;

        public StripePaymentServices(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _stripeSecretKey = config["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task<Session> CreateCheckoutSessionAsync(string userId)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.Query()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.CartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Images = new List<string> { item.Product.ImageUrl }
                        }
                    },
                    Quantity = item.Quantity,
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{GetBaseUrl()}/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{GetBaseUrl()}/checkout/cancel",
                Metadata = new Dictionary<string, string> { { "user_id", userId } },
                CustomerEmail = GetUserEmail(userId) // Optional if you want pre-filled email
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }

        public async Task HandlePaymentSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                await CreateOrderFromPayment(session);
            }
        }

        private async Task CreateOrderFromPayment(Session session)
        {
            var userId = session.Metadata["user_id"];
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var orderRepo = _unitOfWork.GetRepository<Orders>();
            var paymentRepo = _unitOfWork.GetRepository<Payment>();

            var cart = await cartRepo.Query()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var addressRepo = _unitOfWork.GetRepository<Domain.Models.Address>();
            var defaultAddress = await addressRepo.Query()
                .FirstOrDefaultAsync(a => a.Customer_Id.ToString() == userId && a.IsDefault);

            // Create order
            var order = new Orders
            {
                Customer_Id = int.Parse(userId),
                Order_Date = DateTime.UtcNow,
                Total_Amount = cart.CartItems.Sum(i => i.Product.Price * i.Quantity),
                Shipping_Address_Id = defaultAddress?.Address_Id ?? 0,
                Order_Status = "Processing"
            };

            await orderRepo.AddAsync(order);

            // Create payment record
            await paymentRepo.AddAsync(new Payment
            {
                Oreder_Id = order.Order_Id,
                Stripe_payment_intent_id = session.PaymentIntentId,
                Amount = order.Total_Amount,
                Payment_type = "card",
                Payment_date = DateTime.UtcNow,
                Status = "completed"
            });

            // Clear cart
            cart.CartItems.Clear();
            await _unitOfWork.CommitAsync();
        }

        private string GetBaseUrl()
        {
            // Implement based on your environment
            return "https://yourdomain.com";
        }

        private string GetUserEmail(string userId)
        {
            // Implement user email lookup
            return "customer@example.com";
        }
    }
}
