using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class AddressServices
    {
        private readonly IRepository<Address> _addressRepo;
        private readonly Action<string> _logAction;

        public AddressServices(IRepository<Address> addressRepo)
        {
            _addressRepo = addressRepo;
            _logAction = message => Console.WriteLine($"Log: {message}");
        }

        // Get all addresses
        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepo.GetAllAsync();
        }

        // Get address by ID
        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _addressRepo.GetByIdAsync(id);
        }

        // Add a new address
        public async Task AddAddressAsync(Address address)
        {
            await _addressRepo.AddAsync(address, _logAction);
        }

        // Update an existing address
        public async Task UpdateAddressAsync(Address address)
        {
            await _addressRepo.UpdateAsync(address, _logAction);
        }

        // Delete an address
        public async Task DeleteAddressAsync(int id)
        {
            await _addressRepo.DeleteAsync(id, _logAction);
        }
    }
}
