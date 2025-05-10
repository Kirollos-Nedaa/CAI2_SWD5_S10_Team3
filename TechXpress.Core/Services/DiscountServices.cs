using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class DiscountService
    {
        private readonly IRepository<Discount> _discountRepo;
        private readonly Action<string> _logAction;

        public DiscountService(IRepository<Discount> discountRepo)
        {
            _discountRepo = discountRepo;
            _logAction = message => Console.WriteLine($"Log: {message}");
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
        {
            return await _discountRepo.GetAllAsync();
        }

        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            return await _discountRepo.GetByIdAsync(id);
        }

        public async Task AddDiscountAsync(Discount discount)
        {
            await _discountRepo.AddAsync(discount, _logAction);
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            await _discountRepo.UpdateAsync(discount, _logAction);
        }

        public async Task DeleteDiscountAsync(int id)
        {
            await _discountRepo.DeleteAsync(id, _logAction);
        }
    }
}