using Microsoft.AspNetCore.Mvc;
using TechXpress.Domain.Models;
using TechXpress.Core.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TechXpress.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {

        // GET: api/address
        [HttpGet]
        public ActionResult<IEnumerable<Address>> GetAll()
        {
            return View();
        }

        // GET: api/address/1
        [HttpGet("{id}")]
        public ActionResult<Address> GetById(int id)
        {
            var address = addresses.FirstOrDefault(a => a.Address_Id == id);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        // POST: api/address
        [HttpPost]
        public ActionResult<Address> Create(Address address)
        {
            address.Address_Id = addresses.Count > 0 ? addresses.Max(a => a.Address_Id) + 1 : 1;
            addresses.Add(address);
            return CreatedAtAction(nameof(GetById), new { id = address.Address_Id }, address);
        }

        // PUT: api/address/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, Address updated)
        {
            var address = addresses.FirstOrDefault(a => a.Address_Id == id);
            if (address == null)
                return NotFound();

            address.Country = updated.Country;
            address.City = updated.City;
            address.Apartment = updated.Apartment;
            address.PostCode = updated.PostCode;
            address.IsDefault = updated.IsDefault;
            address.Customer_Id = updated.Customer_Id;

            return NoContent();
        }

        // DELETE: api/address/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var address = addresses.FirstOrDefault(a => a.Address_Id == id);
            if (address == null)
                return NotFound();

            addresses.Remove(address);
            return NoContent();
        }
    }
}
