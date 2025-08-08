using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Infrastructure.Contexts;

namespace TechXpress.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products
            .Select(p => new {
                id = p.Product_Id,
                name = p.Name,
                icon = p.ImageUrl,
                price = p.Price,
                brand = p.Brand.Name,
                category = p.Category.Name,
                description = p.Description,
                stock = p.Quantity
            }).ToListAsync();

            return Ok(products);
        }
    }
}
