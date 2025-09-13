using System;
using System.Collections.Generic;
using CoffeeClub.Config;
using CoffeeClub.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CoffeeClub.Functions;

public class CoffeesUpdated
{
    private readonly ILogger<CoffeesUpdated> _logger;

    public CoffeesUpdated(ILogger<CoffeesUpdated> logger)
    {
        _logger = logger;
    }

    [Function("CoffeesUpdated")]
    public void Run([CosmosDBTrigger(
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
    }
}
