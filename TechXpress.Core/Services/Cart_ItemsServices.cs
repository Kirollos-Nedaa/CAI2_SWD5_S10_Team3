using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class Cart_ItemsServices
    {
        //private readonly IRepository<Cart_Items> _cartItemsRepo;
        //private readonly Action<string> _logAction;

        //public Cart_ItemsServices(IRepository<Cart_Items> cartItemsRepo)
        //{
        //    _cartItemsRepo = cartItemsRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //// Get all cart items
        //public async Task<IEnumerable<Cart_Items>> GetAllCartItemsAsync()
        //{
        //    return await _cartItemsRepo.GetAllAsync();
        //}

        //// Get cart item by ID
        //public async Task<Cart_Items> GetCartItemByIdAsync(int id)
        //{
        //    return await _cartItemsRepo.GetByIdAsync(id);
        //}

        //// Add a cart item
        //public async Task AddCartItemAsync(Cart_Items item)
        //{
        //    await _cartItemsRepo.AddAsync(item, _logAction);
        //}

        //// Update a cart item
        //public async Task UpdateCartItemAsync(Cart_Items item)
        //{
        //    await _cartItemsRepo.UpdateAsync(item, _logAction);
        //}

        //// Delete a cart item
        //public async Task DeleteCartItemAsync(int id)
        //{
        //    await _cartItemsRepo.DeleteAsync(id, _logAction);
        //}
    }
}
