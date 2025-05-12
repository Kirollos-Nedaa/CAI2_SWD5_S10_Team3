using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class CustomerController : Controller
    {
        //private readonly CustomerServices _customerService;

        //public CustomerController(CustomerServices customerService)
        //{
        //    _customerService = customerService;
        //}

        //public async Task<IActionResult> Index() => View(await _customerService.GetAllCustomersAsync());
        //public async Task<IActionResult> Details(int id) => View(await _customerService.GetCustomerByIdAsync(id));

        //[HttpPost]
        //public async Task<IActionResult> Create(Customer customer)
        //{
        //    await _customerService.AddCustomerAsync(customer);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Customer customer)
        //{
        //    await _customerService.UpdateCustomerAsync(customer);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _customerService.DeleteCustomerAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}