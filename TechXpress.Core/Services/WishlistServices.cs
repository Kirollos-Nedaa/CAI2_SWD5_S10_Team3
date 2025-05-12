using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class WishlistService
    {
        //private readonly IRepository<Wishlist> _wishlistRepo;
        //private readonly Action<string> _logAction;

        //public WishlistService(IRepository<Wishlist> wishlistRepo)
        //{
        //    _wishlistRepo = wishlistRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //public async Task<IEnumerable<Wishlist>> GetAllWishlistsAsync()
        //{
        //    return await _wishlistRepo.GetAllAsync();
        //}

        //public async Task<Wishlist> GetWishlistByIdAsync(int id)
        //{
        //    return await _wishlistRepo.GetByIdAsync(id);
        //}

        //public async Task AddWishlistAsync(Wishlist wishlist)
        //{
        //    await _wishlistRepo.AddAsync(wishlist, _logAction);
        //}

        //public async Task UpdateWishlistAsync(Wishlist wishlist)
        //{
        //    await _wishlistRepo.UpdateAsync(wishlist, _logAction);
        //}

        //public async Task DeleteWishlistAsync(int id)
        //{
        //    await _wishlistRepo.DeleteAsync(id, _logAction);
        //}
    }
}