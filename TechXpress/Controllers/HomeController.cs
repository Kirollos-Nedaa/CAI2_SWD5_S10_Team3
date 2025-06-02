using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure;
using TechXpress.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var productRepository = _unitOfWork.GetRepository<Product>();
            var wishlistRepository = _unitOfWork.GetRepository<Wishlist>();

            var categories = await categoryRepository.GetAllAsync();
            var products = await productRepository.GetAllAsync();

            var viewModel = new HomeViewModel
            {
                Categories = categories.ToList(),
                Products = products.ToList()
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allWishlistItems = await wishlistRepository.GetAllAsync();

                var userWishlistItems = allWishlistItems
                    .Where(w => w.Customer_Id == userId)
                    .Select(w => w.Product_Id)
                    .ToList();

                viewModel.WishlistProductIds = new HashSet<int>(userWishlistItems);
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page data");
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}