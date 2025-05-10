using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Orders order)
        {
            await _orderService.AddOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Orders order)
        {
            await _orderService.UpdateOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}