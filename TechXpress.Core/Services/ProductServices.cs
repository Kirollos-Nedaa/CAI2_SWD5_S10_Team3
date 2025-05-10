using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepo;
        private readonly Action<string> _logAction;

        public ProductService(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
            _logAction = message => Console.WriteLine($"Log: {message}");
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepo.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepo.AddAsync(product, _logAction);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepo.UpdateAsync(product, _logAction);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepo.DeleteAsync(id, _logAction);
        }
    }
}