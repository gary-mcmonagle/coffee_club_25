using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using CoffeeClub.Config;
using CoffeeClub.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CoffeeClub.Functions;

public class CoffeesUpdated
{
    private readonly ILogger<CoffeesUpdated> _logger;
    private readonly ServiceBusClient _serviceBusClient;

    public CoffeesUpdated(ILogger<CoffeesUpdated> logger, ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        _serviceBusClient = serviceBusClient;
    }

    [Function("CoffeesUpdated")]

    public async Task Run([CosmosDBTrigger(
        databaseName: CoffeeClubConfiguration.Cosmos.DatabaseName,
        containerName: CoffeeClubConfiguration.Cosmos.ContainerName,
        Connection = "coffees",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<CoffeeEntity> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
        }
        foreach (var coffee in input)
        {
            _logger.LogInformation($"Coffee Id: {coffee.Id}, Name: {coffee.Name}");
        }

        var sender = _serviceBusClient.CreateSender("coffee-queue");
        foreach (var coffee in input)
        {
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(coffee))
            {
                Subject = "CoffeeUpdated",
                MessageId = coffee.Id.ToString(),
                ApplicationProperties =
                {
                    { "CoffeeId", coffee.Id },
                    { "CoffeeName", coffee.Name },
                    { "UpdatedAt", DateTime.UtcNow }
                }
            };
            await sender.SendMessageAsync(message);
            _logger.LogInformation($"Sent message for Coffee Id: {coffee.Id}, Name: {coffee.Name}");
        }
        // return input.ToList();
    }
}
