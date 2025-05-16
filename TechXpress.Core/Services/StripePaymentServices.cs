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
using Microsoft.AspNetCore.Http;

namespace TechXpress.Core.Services
{
    public class StripePaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _stripeSecretKey;
        private IConfiguration config;
        private CartServices cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StripePaymentServices(IUnitOfWork unitOfWork, IConfiguration config, IHttpContextAccessor httpContextAccessor, CartServices cartServices)
        {
            _unitOfWork = unitOfWork;
            _stripeSecretKey = config["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
            _httpContextAccessor = httpContextAccessor;
            cartService = cartServices;
        }

        public StripePaymentServices(IConfiguration config, CartServices cartService)
        {
            this.config = config;
            this.cartService = cartService;
        }

        public async Task<Session> CreateCheckoutSessionAsync(string userId, HttpRequest request)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.Query()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            var baseUrl = $"{request.Scheme}://{request.Host.Value}";

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
                            Images = string.IsNullOrWhiteSpace(item.Product.ImageUrl)
                                ? null
                                : new List<string> { $"{baseUrl}/{item.Product.ImageUrl.TrimStart('/')}" }
                        }
                    },
                    Quantity = item.Quantity,
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{baseUrl}/order/confirm?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{baseUrl}/order/paymentfailed",
                Metadata = new Dictionary<string, string> { { "user_id", userId } },
                CustomerEmail = GetUserEmail(userId)
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
            var addressRepo = _unitOfWork.GetRepository<Domain.Models.Address>();

            var cart = await cartRepo.Query()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var defaultAddress = await addressRepo.Query()
                .FirstOrDefaultAsync(a => a.ApplicationUserId == userId && a.IsDefault);

            if (defaultAddress == null)
            {
                defaultAddress = new Domain.Models.Address
                {
                    ApplicationUserId = userId,
                    Country = "Temporary Country",
                    City = "Temporary City",
                    Apartment = "Temporary Apartment",
                    PostCode = "00000",
                    IsDefault = true
                };

                await addressRepo.AddAsync(defaultAddress);
                await _unitOfWork.CommitAsync();
            }

            var order = new Orders
            {
                ApplicationUserId = userId,
                Order_Date = DateTime.UtcNow,
                Total_Amount = cart.CartItems.Sum(i => i.Product.Price * i.Quantity),
                Shipping_Address_Id = defaultAddress.Address_Id,
                Order_Status = "Processing"
            };

            await orderRepo.AddAsync(order);
            await _unitOfWork.CommitAsync();

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

        public async Task<bool> VerifyPayment(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            return session.PaymentStatus == "paid";
        }

        private string GetBaseUrl(HttpRequest request)
        {
            var scheme = request.Scheme; // http or https
            var host = request.Host.Value;
            return $"{scheme}://{host}";
        }

        private string GetUserEmail(string userId)
        {
            // Implement user email lookup
            return "customer@example.com";
        }
    }
}
