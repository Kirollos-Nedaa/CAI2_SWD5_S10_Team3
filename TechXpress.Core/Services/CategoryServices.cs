using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        private readonly S3Service _s3Service;

        public CategoryService(
            IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger,
            S3Service s3Service)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _s3Service = s3Service;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Category>();
                return await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                throw new ApplicationException("Error retrieving categories", ex);
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id, bool includeRelated = true)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Category>();
                return includeRelated
                    ? await repository.Query().FirstOrDefaultAsync(c => c.Category_Id == id)
                    : await repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {id}");
                throw new ApplicationException($"Error retrieving category with ID {id}", ex);
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                ValidateCategory(category);

                var repository = _unitOfWork.GetRepository<Category>();
                await repository.AddAsync(category);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Category {category.Name} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                throw new ApplicationException("Error adding category", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                ValidateCategory(category);

                var existingCategory = await GetCategoryByIdAsync(category.Category_Id);
                if (existingCategory == null)
                {
                    throw new ArgumentException($"Category with ID {category.Category_Id} not found");
                }

                await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Category {category.Category_Id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category {category.Category_Id}");
                throw new ApplicationException($"Error updating category {category.Category_Id}", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await GetCategoryByIdAsync(id);
                if (!string.IsNullOrWhiteSpace(category.Img))
                {
                    await _s3Service.DeleteFileAsync(category.Img);
                    _logger.LogInformation($"Image deleted from S3 for category {id}");
                }

                await _unitOfWork.GetRepository<Category>().DeleteAsync(category);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Category {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category {id}");
                throw new ApplicationException($"Error deleting category {id}", ex);
            }
        }

        private void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name is required");

            if (category.Name.Length > 50)
                throw new ArgumentException("Category name cannot exceed 50 characters");
        }
    }
}