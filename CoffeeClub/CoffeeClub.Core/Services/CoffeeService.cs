using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using CoffeeClub.Domain.Extensions;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using CoffeeClub.Domain.Entities;

namespace CoffeeClub.Core.Services;

public class CoffeeService(Container coffeeContainer, ServiceBusClient serviceBusClient) : ICoffeeService
{

    private const string QueueName = "coffee-queue";
    private readonly Container _coffeeContainer = coffeeContainer;
    private readonly ServiceBusSender _serviceBusSender = serviceBusClient.CreateSender(QueueName);

    public async Task<List<CoffeeDto>> GetCoffeesAsync()
    {
        var query = _coffeeContainer.GetItemLinqQueryable<CoffeeDto>();
        var iterator = query.ToFeedIterator();
        var results = new List<CoffeeDto>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }
        return results;
    }

    public async Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto coffeeDto)
    {
        var coffeeEntity = coffeeDto.ToEntity();
        // Save to Cosmos DB

        await _coffeeContainer.CreateItemAsync(coffeeEntity, new PartitionKey(coffeeEntity.Id.ToString()), new ItemRequestOptions {
            
        });

        // Send a message to the Service Bus queue
        // var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(coffee))
        // {
        //     ContentType = "application/json",
        //     Subject = "NewCoffee",
        //     MessageId = coffee.Id
        // };
        // await _serviceBusSender.SendMessageAsync(message);

        return coffeeEntity.ToDto();
    }

    public async Task<CoffeeDto?> GetCoffeeAsync(Guid id)
    {
        var response = await _coffeeContainer.ReadItemAsync<CoffeeEntity>(id.ToString(), new PartitionKey(id.ToString()));
        return response.Resource?.ToDto();
    }

    public async Task<bool> DeleteCoffeeAsync(Guid id)
    {
        var response = await _coffeeContainer.DeleteItemAsync<CoffeeEntity>(id.ToString(), new PartitionKey(id.ToString()));
        return response.StatusCode == System.Net.HttpStatusCode.NoContent;
    }
}
