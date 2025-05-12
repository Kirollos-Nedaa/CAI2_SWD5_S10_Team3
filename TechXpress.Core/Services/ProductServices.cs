using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeRelated = true)
        {
            try
            {
                var query = _unitOfWork.GetRepository<Product>().Query();

                if (includeRelated)
                {
                    query = query.Include(p => p.Category)
                                 .Include(p => p.Brand);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                throw new ApplicationException("Error retrieving products", ex);
            }
        }

        public async Task<Product> GetProductByIdAsync(int id, bool includeRelated = true)
        {
            try
            {
                var query = _unitOfWork.GetRepository<Product>().Query()
                    .Where(p => p.Product_Id == id);

                if (includeRelated)
                {
                    query = query.Include(p => p.Category)
                                 .Include(p => p.Brand);
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with ID {id}");
                throw new ApplicationException($"Error retrieving product with ID {id}", ex);
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                ValidateProduct(product);

                await _unitOfWork.GetRepository<Product>().AddAsync(product);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Product {product.Name} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                throw new ApplicationException("Error adding product", ex);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                ValidateProduct(product);

                var existingProduct = await GetProductByIdAsync(product.Product_Id);
                if (existingProduct == null)
                {
                    throw new ArgumentException($"Product with ID {product.Product_Id} not found");
                }

                await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Product {product.Product_Id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product {product.Product_Id}");
                throw new ApplicationException($"Error updating product {product.Product_Id}", ex);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await GetProductByIdAsync(id);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {id} not found");
                }

              await  _unitOfWork.GetRepository<Product>().DeleteAsync(product);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Product {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                throw new ApplicationException($"Error deleting product {id}", ex);
            }
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required");

            if (product.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero");

            if (product.Quantity < 0)
                throw new ArgumentException("Product quantity cannot be negative");
        }
    }
}