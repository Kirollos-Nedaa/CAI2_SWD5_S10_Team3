using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure.Contexts;

namespace TechXpress.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddressesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var user = await _userManager.GetUserAsync(User);

            var addresses = await _context.Addresses
                .Where(a => a.ApplicationUserId == user.Id)
                .Select(a => new AddressDto
                {
                    Address_Id = a.Address_Id,
                    FullName = a.FullName,
                    AddressType = a.AddressType,
                    Country = a.Country,
                    City = a.City,
                    StreetAddress = a.StreetAddress,
                    Buillding = a.Buillding,
                    Floor = a.Floor,
                    Flat = a.Flat,
                    PostCode = a.PostCode,
                    IsDefault = a.IsDefault
                })
                .ToListAsync();

            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var address = await _context.Addresses
                .Where(a => a.Address_Id == id && a.ApplicationUserId == user.Id)
                .Select(a => new AddressDto
                {
                    Address_Id = a.Address_Id,
                    FullName = a.FullName,
                    AddressType = a.AddressType,
                    Country = a.Country,
                    City = a.City,
                    StreetAddress = a.StreetAddress,
                    Buillding = a.Buillding,
                    Floor = a.Floor,
                    Flat = a.Flat,
                    PostCode = a.PostCode,
                    IsDefault = a.IsDefault
                })
                .FirstOrDefaultAsync();

            if (address == null)
                return NotFound("Address not found.");

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressDto dto)
        {
            var user = await _userManager.GetUserAsync(User);

            var address = new Address
            {
                ApplicationUserId = user.Id,
                FullName = dto.FullName,
                AddressType = dto.AddressType,
                Country = dto.Country,
                City = dto.City,
                StreetAddress = dto.StreetAddress,
                Buillding = dto.Buillding,
                Floor = dto.Floor,
                Flat = dto.Flat,
                PostCode = dto.PostCode,
                IsDefault = dto.IsDefault
            };

            if (address.IsDefault)
            {
                var existingDefaults = await _context.Addresses
                    .Where(a => a.ApplicationUserId == user.Id && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                {
                    addr.IsDefault = false;
                }
            }

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok(address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, AddressDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Address_Id == id && a.ApplicationUserId == user.Id);

            if (address == null)
                return NotFound("Address not found.");

            address.FullName = dto.FullName;
            address.AddressType = dto.AddressType;
            address.Country = dto.Country;
            address.City = dto.City;
            address.StreetAddress = dto.StreetAddress;
            address.Buillding = dto.Buillding;
            address.Floor = dto.Floor;
            address.Flat = dto.Flat;
            address.PostCode = dto.PostCode;

            if (dto.IsDefault)
            {
                var existingDefaults = await _context.Addresses
                    .Where(a => a.ApplicationUserId == user.Id && a.IsDefault && a.Address_Id != address.Address_Id)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                {
                    addr.IsDefault = false;
                }

                address.IsDefault = true;
            }

            await _context.SaveChangesAsync();
            return Ok(address);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Address_Id == id && a.ApplicationUserId == user.Id);

            if (address == null)
                return NotFound("Address not found.");

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return Ok("Address deleted.");
        }
    }
}
