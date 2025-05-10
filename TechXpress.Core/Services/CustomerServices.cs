using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Infrastructure;
using TechXpress.Domain.Models;

namespace TechXpress.Core.Services
{
    public class CustomerServices
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly Action<string> _logAction;

        public CustomerServices(IRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
            _logAction = message => Console.WriteLine($"Log: {message}");
        }

        // Get all customers
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        // Get a customer by ID
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepo.GetByIdAsync(id);
        }

        // Add a new customer
        public async Task AddCustomerAsync(Customer customer)
        {
            await _customerRepo.AddAsync(customer, _logAction);
        }

        // Update a customer
        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepo.UpdateAsync(customer, _logAction);
        }

        // Delete a customer
        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepo.DeleteAsync(id, _logAction);
        }
    }
}
