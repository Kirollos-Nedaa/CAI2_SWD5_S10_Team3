using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class BrandServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandServices> _logger;

        public BrandServices(
            IUnitOfWork unitOfWork,
            ILogger<BrandServices> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Brand>();
                return await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving brands");
                throw new ApplicationException("Error retrieving brands", ex);
            }
        }

        public async Task<Brand> GetBrandByIdAsync(int id, bool includeRelated = true)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Brand>();
                return includeRelated
                    ? await repository.Query().FirstOrDefaultAsync(b => b.Brand_Id == id)
                    : await repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving brand with ID {id}");
                throw new ApplicationException($"Error retrieving brand with ID {id}", ex);
            }
        }

        public async Task AddBrandAsync(Brand brand)
        {
            try
            {
                ValidateBrand(brand);

                var repository = _unitOfWork.GetRepository<Brand>();
                await repository.AddAsync(brand);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Brand {brand.Name} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding brand");
                throw new ApplicationException("Error adding brand", ex);
            }
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            try
            {
                ValidateBrand(brand);

                var repository = _unitOfWork.GetRepository<Brand>();
               await repository.UpdateAsync(brand);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Brand {brand.Brand_Id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating brand {brand.Brand_Id}");
                throw new ApplicationException($"Error updating brand {brand.Brand_Id}", ex);
            }
        }

        public async Task DeleteBrandAsync(int id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Brand>();
                var brand = await repository.GetByIdAsync(id);

                if (brand == null)
                {
                    throw new ArgumentException($"Brand with ID {id} not found");
                }

               await repository.DeleteAsync(brand);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Brand {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting brand {id}");
                throw new ApplicationException($"Error deleting brand {id}", ex);
            }
        }

        private void ValidateBrand(Brand brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));

            if (string.IsNullOrWhiteSpace(brand.Name))
                throw new ArgumentException("Brand name is required");

            if (brand.Name.Length > 50)
                throw new ArgumentException("Brand name cannot exceed 50 characters");
        }
    }
}