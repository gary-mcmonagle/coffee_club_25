
using Microsoft.EntityFrameworkCore;

namespace CoffeeClub.ApiService.Infrastructure;

public class CoffeeService(CoffeeClubDbContext dbContext) : ICoffeeService
{
    public Task AddCoffeeAsync(Coffee coffee)
    {
        dbContext.Coffees.Add(coffee);
        return dbContext.SaveChangesAsync();
    }

    public Task DeleteCoffeeAsync(string id)
    {
        var coffee = dbContext.Coffees.Find(id);
        if (coffee != null)
        {
            dbContext.Coffees.Remove(coffee);
            return dbContext.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }

    public  async Task<Coffee?> GetCoffeeByIdAsync(string id)
    {
        return await dbContext.Coffees.FindAsync(id);
    }

    public async Task<IEnumerable<Coffee>> GetCoffeesAsync()
    {
        return await dbContext.Coffees.ToListAsync();
    }

    public async Task UpdateCoffeeAsync(Coffee coffee)
    {
        dbContext.Coffees.Update(coffee);
        await dbContext.SaveChangesAsync();
    }
}
