using Microsoft.AspNetCore.Mvc;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using System.Threading.Tasks;

namespace TechXpress.Web.Controllers
{
    public class AddressController : Controller
    {
        private readonly AddressServices _addressServices;

        public AddressController(AddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        public async Task<IActionResult> Index()
        {
            var addresses = await _addressServices.GetAllAddressesAsync();
            return View(addresses);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Address address)
        {
            await _addressServices.AddAddressAsync(address);
            var addresses = await _addressServices.GetAllAddressesAsync();
            return View(nameof(Index), addresses);
        }
    }
}