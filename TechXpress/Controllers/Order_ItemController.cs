using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class OrderItemController : Controller
    {
        //private readonly OrderItemService _orderItemService;

        //public OrderItemController(OrderItemService orderItemService)
        //{
        //    _orderItemService = orderItemService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var orderItems = await _orderItemService.GetAllOrderItemsAsync();
        //    return View(orderItems);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
        //    return View(orderItem);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Order_Item orderItem)
        //{
        //    await _orderItemService.AddOrderItemAsync(orderItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Order_Item orderItem)
        //{
        //    await _orderItemService.UpdateOrderItemAsync(orderItem);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _orderItemService.DeleteOrderItemAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}