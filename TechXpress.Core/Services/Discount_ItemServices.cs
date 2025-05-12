using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class DiscountItemService
    {
        //private readonly IRepository<Discount_Item> _discountItemRepo;
        //private readonly Action<string> _logAction;

        //public DiscountItemService(IRepository<Discount_Item> discountItemRepo)
        //{
        //    _discountItemRepo = discountItemRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //public async Task<IEnumerable<Discount_Item>> GetAllDiscountItemsAsync()
        //{
        //    return await _discountItemRepo.GetAllAsync();
        //}

        //public async Task<Discount_Item> GetDiscountItemByIdAsync(int id)
        //{
        //    return await _discountItemRepo.GetByIdAsync(id);
        //}

        //public async Task AddDiscountItemAsync(Discount_Item discountItem)
        //{
        //    await _discountItemRepo.AddAsync(discountItem, _logAction);
        //}

        //public async Task UpdateDiscountItemAsync(Discount_Item discountItem)
        //{
        //    await _discountItemRepo.UpdateAsync(discountItem, _logAction);
        //}

        //public async Task DeleteDiscountItemAsync(int id)
        //{
        //    await _discountItemRepo.DeleteAsync(id, _logAction);
        //}
    }
}