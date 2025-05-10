using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class CategoryServices
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly Action<string> _logAction;


        public CategoryServices(IRepository<Category> CategoryRepo)
        {
            _categoryRepo = CategoryRepo;
            _logAction = message => Console.WriteLine($"Log: {message}");
        }

        // Get all categories
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepo.GetAllAsync();
        }

        // Get category by ID
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepo.GetByIdAsync(id);
        }

        // Add a new category
        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepo.AddAsync(category, _logAction);
        }

        // Update an existing category
        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepo.UpdateAsync(category, _logAction);
        }

        // Delete a category
        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepo.DeleteAsync(id, _logAction);
        }
    }
}
