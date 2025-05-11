using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CategoryServices _categoryService;
    private readonly ProductServices _productService;

    public HomeController(ProductServices productService, CategoryServices categoryServices)
    {
        _productService = productService;
        _categoryService = categoryServices;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var products = await _productService.GetAllProductsAsync();

        var viewModel = new HomeViewModel
        {
            Categories = categories.ToList(),
            Products = products.ToList()
        };

        return View(viewModel);
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
