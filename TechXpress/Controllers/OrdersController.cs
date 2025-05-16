using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure;

namespace TechXpress.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CartServices _cartService;
        private readonly IConfiguration _config;

        public OrderController(
            IUnitOfWork unitOfWork,
            CartServices cartService,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _config = config;
        }

        [Authorize]
        public async Task<IActionResult> Confirm(string session_id)
        {
            try
            {
                var stripeService = new StripePaymentServices(_config, _cartService);
                if (await stripeService.VerifyPayment(session_id))
                {
                    await CreateOrder(session_id);
                    return View("Success");
                }
                return RedirectToAction("PaymentFailed");
            }
            catch (StripeException e)
            {
                return RedirectToAction("PaymentFailed");
            }
        }

        private async Task CreateOrder(string sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync();
            var addressRepo = _unitOfWork.GetRepository<Domain.Models.Address>();

            // Try to find an existing default address
            var address = await addressRepo.Query()
                .FirstOrDefaultAsync(a => a.ApplicationUserId == userId && a.IsDefault);

            if (address == null)
            {
                address = new Domain.Models.Address
                {
                    ApplicationUserId = userId,
                    Country = "Temporary Country",
                    City = "Temporary City",
                    Apartment = "Temporary State",
                    PostCode = "00000",
                    IsDefault = true
                };

                await addressRepo.AddAsync(address);
                await _unitOfWork.CommitAsync();
            }

            var order = new Orders
            {
                ApplicationUserId = userId,
                Order_Date = DateTime.UtcNow,
                Total_Amount = cart.CartItems.Sum(i => i.Product.Price * i.Quantity),
                Shipping_Address_Id = address.Address_Id,
                Order_Status = "Paid"
            };

            var orderRepo = _unitOfWork.GetRepository<Orders>();
            await orderRepo.AddAsync(order);
            await _unitOfWork.CommitAsync();

            // Clear cart
            await _cartService.ClearCartAsync();
        }

        public IActionResult PaymentFailed()
        {
            return View();
        }

        public async Task<IActionResult> OrderHistory(int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            int pageSize = 5;

            var orderRepo = _unitOfWork.GetRepository<Orders>();
            var query = orderRepo.Query()
                .Where(o => o.ApplicationUserId == userId)
                .OrderByDescending(o => o.Order_Date);

            var totalOrders = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.Order_Id,
                    Status = o.Order_Status,
                    OrderDate = o.Order_Date,
                    TotalAmount = o.Total_Amount
                })
                .ToListAsync();

            var viewModel = new OrderHistoryViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public IActionResult OrderDetails()
        {
            return View();
        }
    }
}