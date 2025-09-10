using CoffeeClub.Core.Infrastructure;
using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using CoffeeClub.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeClub.Core.Services;

public class CoffeeService(CoffeeContext context) : ICoffeeService
{
    // private readonly List<CoffeeDto> _coffees = new()
    // {
    //     new CoffeeDto { Id = Guid.NewGuid(), Name = "Espresso", Roast = "Dark" },
    //     new CoffeeDto { Id = Guid.NewGuid(), Name = "Latte", Roast = "Medium" },
    //     new CoffeeDto { Id = Guid.NewGuid(), Name = "Cappuccino", Roast = "Light" }
    // };

    public async Task<List<CoffeeDto>> GetCoffeesAsync()
    {
        return (await context.Coffees.ToListAsync())
            .Select(c => c.ToDto())
            .ToList();
    }

    public async Task<CoffeeDto?> GetCoffeeAsync(Guid id)
    {
        var coffee = await context.Coffees.FindAsync(id);
        return coffee?.ToDto();
    }

    public async Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto createCoffeeDto)
    {
        var entity = createCoffeeDto.ToEntity();
        context.Coffees.Add(entity);
        await context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task<bool> DeleteCoffeeAsync(Guid id)
    {
        var coffee = await context.Coffees.FindAsync(id);
        if (coffee != null)
        {
            context.Coffees.Remove(coffee);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}
