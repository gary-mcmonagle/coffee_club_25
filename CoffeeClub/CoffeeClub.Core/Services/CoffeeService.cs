using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;

namespace CoffeeClub.Core.Services;

public class CoffeeService : ICoffeeService
{
    private readonly List<CoffeeDto> _coffees = new()
    {
        new CoffeeDto { Id = Guid.NewGuid(), Name = "Espresso", Roast = "Dark" },
        new CoffeeDto { Id = Guid.NewGuid(), Name = "Latte", Roast = "Medium" },
        new CoffeeDto { Id = Guid.NewGuid(), Name = "Cappuccino", Roast = "Light" }
    };

    public Task<List<CoffeeDto>> GetCoffeesAsync()
    {
        return Task.FromResult(_coffees);
    }

    public Task<CoffeeDto?> GetCoffeeAsync(Guid id)
    {
        var coffee = _coffees.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(coffee);
    }

    public Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto createCoffeeDto)
    {
        var coffee = new CoffeeDto
        {
            Id = Guid.NewGuid(),
            Name = createCoffeeDto.Name
        };
        _coffees.Add(coffee);
        return Task.FromResult(coffee);
    }

    public Task<bool> DeleteCoffeeAsync(Guid id)
    {
        var coffee = _coffees.FirstOrDefault(c => c.Id == id);
        if (coffee != null)
        {
            _coffees.Remove(coffee);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
