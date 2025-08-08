using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Infrastructure.Contexts;


namespace TechXpress.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
            .Select(c => new {
                id = c.Category_Id,
                name = c.Name,
                icon = c.Img
            }).ToListAsync();

            return Ok(categories);
        }
    }
}
