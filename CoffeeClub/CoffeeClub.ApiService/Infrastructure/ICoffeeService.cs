using CoffeeClub.Domain;

namespace CoffeeClub.ApiService.Infrastructure;

public interface ICoffeeService
{
    Task<IEnumerable<CoffeeClubCoffeeModel>> GetCoffeesAsync();
    Task<CoffeeClubCoffeeModel?> GetCoffeeByIdAsync(string id);
    Task AddCoffeeAsync(CoffeeClubCoffeeModel coffee);
    Task UpdateCoffeeAsync(CoffeeClubCoffeeModel coffee);
    Task DeleteCoffeeAsync(string id);
}
