using CoffeeClub.Domain.Dtos;

namespace CoffeeClub.Domain.Services;

public interface ICoffeeService
{
    Task<List<CoffeeDto>> GetCoffeesAsync();
    Task<CoffeeDto?> GetCoffeeAsync(Guid id);
    Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto createCoffeeDto);
    Task<bool> DeleteCoffeeAsync(Guid id);
}
