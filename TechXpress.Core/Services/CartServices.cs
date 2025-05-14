using TechXpress.Domain.Models;
using TechXpress.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class CartServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private const string SessionKey = "Cart";

    public CartServices(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }


    private void SetSession<T>(string key, T value)
    {
        _httpContextAccessor.HttpContext.Session.SetString(key, JsonSerializer.Serialize(value));
    }

    private T GetSession<T>(string key)
    {
        var value = _httpContextAccessor.HttpContext.Session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }



    public async Task AddToCartAsync(int productId, int quantity)
    {
        var user = _httpContextAccessor.HttpContext.User;
        var productRepo = _unitOfWork.GetRepository<Product>();

        if (user.Identity.IsAuthenticated)
        {
            await AddToDatabaseCart(user.FindFirstValue(ClaimTypes.NameIdentifier), productId, quantity);
        }
        else
        {
            AddToSessionCart(productId, quantity);
        }
    }

    private async Task AddToDatabaseCart(string userId, int productId, int quantity)
    {
        var cartRepo = _unitOfWork.GetRepository<Cart>();
        var cartItemRepo = _unitOfWork.GetRepository<Cart_Items>();

        var cart = await cartRepo.Query()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await cartRepo.AddAsync(cart);
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.Product_Id == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.CartItems.Add(new Cart_Items
            {
                Product_Id = productId,
                Quantity = quantity
            });
        }

        await _unitOfWork.CommitAsync();
    }

    private void AddToSessionCart(int productId, int quantity)
    {
        var cartItems = GetSession<List<Cart_Items>>(SessionKey) ?? new List<Cart_Items>();

        var existingItem = cartItems.FirstOrDefault(ci => ci.Product_Id == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cartItems.Add(new Cart_Items
            {
                Product_Id = productId,
                Quantity = quantity
            });
        }

        SetSession(SessionKey, cartItems);
    }


    public async Task<Cart> GetCartAsync()
    {
        var user = _httpContextAccessor.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            return await GetDatabaseCart(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        return GetSessionCart();
    }

    private async Task<Cart> GetDatabaseCart(string userId)
    {
        var cartRepo = _unitOfWork.GetRepository<Cart>();

        return await cartRepo.Query()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }


    private Cart GetSessionCart()
    {
        var cartItems = GetSession<List<Cart_Items>>(SessionKey) ?? new List<Cart_Items>();

        return new Cart
        {
            CartItems = cartItems,
            UserId = null
        };
    }


    public async Task MergeCartsAsync(string userId)
    {
        var sessionCart = GetSessionCart();
        if (sessionCart.CartItems.Any())
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var dbCart = await cartRepo.Query()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId) ?? new Cart { UserId = userId };

            foreach (var sessionItem in sessionCart.CartItems)
            {
                var existingItem = dbCart.CartItems
                    .FirstOrDefault(ci => ci.Product_Id == sessionItem.Product_Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += sessionItem.Quantity;
                }
                else
                {
                    dbCart.CartItems.Add(new Cart_Items
                    {
                        Product_Id = sessionItem.Product_Id,
                        Quantity = sessionItem.Quantity
                    });
                }
            }

            if (dbCart.Cart_Id == 0)
            {
                await cartRepo.AddAsync(dbCart);
            }

            await _unitOfWork.CommitAsync();
            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
        }
    }


    public async Task RemoveFromCartAsync(int productId)
    {
        var user = _httpContextAccessor.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            await RemoveFromDatabaseCart(user.FindFirstValue(ClaimTypes.NameIdentifier), productId);
        }
        else
        {
            RemoveFromSessionCart(productId);
        }
    }

    private async Task RemoveFromDatabaseCart(string userId, int productId)
    {
        var cartRepo = _unitOfWork.GetRepository<Cart>();
        var cart = await cartRepo.Query()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return;

        var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.Product_Id == productId);
        if (itemToRemove != null)
        {
            var cartItemRepo = _unitOfWork.GetRepository<Cart_Items>();
           await cartItemRepo.DeleteAsync(itemToRemove);
            await _unitOfWork.CommitAsync();
        }
    }

    private void RemoveFromSessionCart(int productId)
    {
        var cartItems = GetSession<List<Cart_Items>>(SessionKey) ?? new List<Cart_Items>();
        var itemToRemove = cartItems.FirstOrDefault(ci => ci.Product_Id == productId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            SetSession(SessionKey, cartItems);
        }
    }

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1");

        var user = _httpContextAccessor.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            await UpdateDatabaseQuantity(user.FindFirstValue(ClaimTypes.NameIdentifier), productId, quantity);
        }
        else
        {
            UpdateSessionQuantity(productId, quantity);
        }
    }

    private async Task UpdateDatabaseQuantity(string userId, int productId, int quantity)
    {
        var cartRepo = _unitOfWork.GetRepository<Cart>();
        var cart = await cartRepo.Query()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return;

        var item = cart.CartItems.FirstOrDefault(ci => ci.Product_Id == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            await _unitOfWork.CommitAsync();
        }
    }

    private void UpdateSessionQuantity(int productId, int quantity)
    {
        var cartItems = GetSession<List<Cart_Items>>(SessionKey);
        if (cartItems == null) return;

        var item = cartItems.FirstOrDefault(ci => ci.Product_Id == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            SetSession(SessionKey, cartItems);
        }
    }

    public async Task<int> GetCartCountAsync()
    {
        var cart = await GetCartAsync();
        return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
    }

    public async Task ClearCartAsync()
    {
        throw new NotImplementedException();
    }
}



