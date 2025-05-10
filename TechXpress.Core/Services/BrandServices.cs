using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

public class BrandServices
{
    private readonly IRepository<Brand> _brandRepo;
    private readonly Action<string> _logAction;

    public BrandServices(IRepository<Brand> BrandRepo)
    {
        _brandRepo = BrandRepo;
        _logAction = message => Console.WriteLine($"Log: {message}");
    }

    // Get all brands
    public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
    {
        return await _brandRepo.GetAllAsync();
    }

    // Get brand by ID
    public async Task<Brand> GetBrandByIdAsync(int id)
    {
        return await _brandRepo.GetByIdAsync(id);
    }

    // Add a new brand
    public async Task AddBrandAsync(Brand brand)
    {
        await _brandRepo.AddAsync(brand, _logAction);
    }

    // Update an existing brand
    public async Task UpdateBrandAsync(Brand brand)
    {
        await _brandRepo.UpdateAsync(brand, _logAction);
    }

    // Delete a brand
    public async Task DeleteBrandAsync(int id)
    {
        await _brandRepo.DeleteAsync(id, _logAction);
    }
}