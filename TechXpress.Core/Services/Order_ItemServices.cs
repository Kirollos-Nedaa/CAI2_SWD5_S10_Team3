using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class OrderItemService
    {
        //private readonly IRepository<Order_Item> _orderItemRepo;
        //private readonly Action<string> _logAction;

        //public OrderItemService(IRepository<Order_Item> orderItemRepo)
        //{
        //    _orderItemRepo = orderItemRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //public async Task<IEnumerable<Order_Item>> GetAllOrderItemsAsync()
        //{
        //    return await _orderItemRepo.GetAllAsync();
        //}

        //public async Task<Order_Item> GetOrderItemByIdAsync(int id)
        //{
        //    return await _orderItemRepo.GetByIdAsync(id);
        //}

        //public async Task AddOrderItemAsync(Order_Item orderItem)
        //{
        //    await _orderItemRepo.AddAsync(orderItem, _logAction);
        //}

        //public async Task UpdateOrderItemAsync(Order_Item orderItem)
        //{
        //    await _orderItemRepo.UpdateAsync(orderItem, _logAction);
        //}

        //public async Task DeleteOrderItemAsync(int id)
        //{
        //    await _orderItemRepo.DeleteAsync(id, _logAction);
        //}
    }
}