using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;

namespace TechXpress.Web.Controllers
{
    public class AddressController : Controller
    {
        private readonly AddressServices _addressService;

        public AddressController(AddressServices addressService)
        {
            _addressService = addressService;
        }

        public async Task<IActionResult> Index() => View(await _addressService.GetAllAddressesAsync());
        public async Task<IActionResult> Details(int id) => View(await _addressService.GetAddressByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Address address)
        {
            await _addressService.AddAddressAsync(address);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Address address)
        {
            await _addressService.UpdateAddressAsync(address);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}