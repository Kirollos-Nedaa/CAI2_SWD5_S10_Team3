using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class OrderService
    {
        //private readonly IRepository<Orders> _orderRepo;
        //private readonly Action<string> _logAction;

        //public OrderService(IRepository<Orders> orderRepo)
        //{
        //    _orderRepo = orderRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //public async Task<IEnumerable<Orders>> GetAllOrdersAsync()
        //{
        //    return await _orderRepo.GetAllAsync();
        //}

        //public async Task<Orders> GetOrderByIdAsync(int id)
        //{
        //    return await _orderRepo.GetByIdAsync(id);
        //}

        //public async Task AddOrderAsync(Orders order)
        //{
        //    await _orderRepo.AddAsync(order, _logAction);
        //}

        //public async Task UpdateOrderAsync(Orders order)
        //{
        //    await _orderRepo.UpdateAsync(order, _logAction);
        //}

        //public async Task DeleteOrderAsync(int id)
        //{
        //    await _orderRepo.DeleteAsync(id, _logAction);
        //}
    }
}