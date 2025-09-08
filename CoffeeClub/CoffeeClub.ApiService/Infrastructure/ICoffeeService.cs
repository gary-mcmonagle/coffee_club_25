namespace CoffeeClub.ApiService.Infrastructure;

public interface ICoffeeService
{
    Task<IEnumerable<Coffee>> GetCoffeesAsync();
    Task<Coffee?> GetCoffeeByIdAsync(string id);
    Task AddCoffeeAsync(Coffee coffee);
    Task UpdateCoffeeAsync(Coffee coffee);
    Task DeleteCoffeeAsync(string id);
}
