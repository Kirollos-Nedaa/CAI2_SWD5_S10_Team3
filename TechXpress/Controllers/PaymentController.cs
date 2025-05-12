using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class PaymentController : Controller
    {
        //private readonly PaymentService _paymentService;

        //public PaymentController(PaymentService paymentService)
        //{
        //    _paymentService = paymentService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var payments = await _paymentService.GetAllPaymentAsync();
        //    return View(payments);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    var payment = await _paymentService.GetPaymentByIdAsync(id);
        //    return View(payment);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Payment payment)
        //{
        //    await _paymentService.AddPaymentAsync(payment);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Payment payment)
        //{
        //    await _paymentService.UpdatePaymentAsync(payment);
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _paymentService.DeletePaymentAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}