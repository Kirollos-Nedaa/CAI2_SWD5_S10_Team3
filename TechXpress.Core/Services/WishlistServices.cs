using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class WishlistService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private const string SessionKey = "Wishlist";

        public WishlistService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
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

        public async Task AddToWishlistAsync(int productId)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                await AddToDatabaseWishlist(userId, productId);
            }
            else
            {
                AddToSessionWishlist(productId);
            }
        }

        private async Task AddToDatabaseWishlist(string userId, int productId)
        {
            var wishlistRepo = _unitOfWork.GetRepository<Wishlist>();

            bool exists = await wishlistRepo.Query()
                .AnyAsync(w => w.Customer_Id == userId && w.Product_Id == productId);

            if (!exists)
            {
                var wishlist = new Wishlist
                {
                    Customer_Id = userId,
                    Product_Id = productId
                };
                await wishlistRepo.AddAsync(wishlist);
                await _unitOfWork.CommitAsync();
            }
        }

        private void AddToSessionWishlist(int productId)
        {
            var wishlist = GetSession<List<int>>(SessionKey) ?? new List<int>();

            if (!wishlist.Contains(productId))
            {
                wishlist.Add(productId);
                SetSession(SessionKey, wishlist);
            }
        }

        public async Task<List<int>> GetWishlistAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishlistRepo = _unitOfWork.GetRepository<Wishlist>();

                return await wishlistRepo.Query()
                    .Where(w => w.Customer_Id == userId)
                    .Select(w => w.Product_Id)
                    .ToListAsync();
            }
            else
            {
                return GetSession<List<int>>(SessionKey) ?? new List<int>();
            }
        }

        public async Task RemoveFromWishlistAsync(int productId)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                await RemoveFromDatabaseWishlist(userId, productId);
            }
            else
            {
                RemoveFromSessionWishlist(productId);
            }
        }

        private async Task RemoveFromDatabaseWishlist(string userId, int productId)
        {
            var wishlistRepo = _unitOfWork.GetRepository<Wishlist>();

            var item = await wishlistRepo.Query()
                .FirstOrDefaultAsync(w => w.Customer_Id == userId && w.Product_Id == productId);

            if (item != null)
            {
                await wishlistRepo.DeleteAsync(item);
                await _unitOfWork.CommitAsync();
            }
        }

        private void RemoveFromSessionWishlist(int productId)
        {
            var wishlist = GetSession<List<int>>(SessionKey) ?? new List<int>();

            if (wishlist.Contains(productId))
            {
                wishlist.Remove(productId);
                SetSession(SessionKey, wishlist);
            }
        }

        public async Task MergeWishlistAsync(string userId)
        {
            var sessionWishlist = GetSession<List<int>>(SessionKey) ?? new List<int>();
            if (!sessionWishlist.Any()) return;

            var wishlistRepo = _unitOfWork.GetRepository<Wishlist>();

            var existingProductIds = await wishlistRepo.Query()
                .Where(w => w.Customer_Id == userId)
                .Select(w => w.Product_Id)
                .ToListAsync();

            var newItems = sessionWishlist.Except(existingProductIds)
                .Select(pid => new Wishlist { Customer_Id = userId, Product_Id = pid })
                .ToList();

            foreach (var item in newItems)
            {
                await wishlistRepo.AddAsync(item);
            }

            if (newItems.Any())
            {
                await _unitOfWork.CommitAsync();
            }

            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
        }

        public async Task<int> GetWishlistCountAsync()
        {
            var wishlist = await GetWishlistAsync();
            return wishlist?.Count ?? 0;
        }

        public async Task ClearWishlistAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishlistRepo = _unitOfWork.GetRepository<Wishlist>();

                var items = await wishlistRepo.Query()
                    .Where(w => w.Customer_Id == userId)
                    .ToListAsync();

                foreach (var item in items)
                {
                    await wishlistRepo.DeleteAsync(item);
                }

                await _unitOfWork.CommitAsync();
            }
            else
            {
                _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
            }
        }
    }
}