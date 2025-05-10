using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

public class CartServices
{
    private readonly IRepository<Cart> _cartRepo;
    private readonly Action<string> _logAction;

    public CartServices(IRepository<Cart> cartRepo)
    {
        _cartRepo = cartRepo;
        _logAction = message => Console.WriteLine($"Log: {message}");
    }

    public async Task<IEnumerable<Cart>> GetAllCartsAsync()
    {
        return await _cartRepo.GetAllAsync();
    }

    public async Task<Cart> GetCartByIdAsync(int id)
    {
        return await _cartRepo.GetByIdAsync(id);
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _cartRepo.AddAsync(cart, _logAction);
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        await _cartRepo.UpdateAsync(cart, _logAction);
    }

    public async Task DeleteCartAsync(int id)
    {
        await _cartRepo.DeleteAsync(id, _logAction);
    }
}