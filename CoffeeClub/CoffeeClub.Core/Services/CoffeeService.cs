using CoffeeClub.Core.Infrastructure;
using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using CoffeeClub.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;

namespace CoffeeClub.Core.Services;

public class CoffeeService(CoffeeContext context, ServiceBusClient serviceBusClient) : ICoffeeService
{
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

        // Send a message to the Service Bus queue
        var sender = serviceBusClient.CreateSender("coffee-queue");
        var message = new ServiceBusMessage($"New coffee added: {entity.Name} (ID: {entity.Id})");
        await sender.SendMessageAsync(message);
        
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
