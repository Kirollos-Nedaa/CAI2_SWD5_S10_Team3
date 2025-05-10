using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CategoryServices _categoryServices;

    public HomeController(ILogger<HomeController> logger, CategoryServices categoryServices)
    {
        _logger = logger;
        _categoryServices = categoryServices;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryServices.GetAllCategoriesAsync();
        return View(categories);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
