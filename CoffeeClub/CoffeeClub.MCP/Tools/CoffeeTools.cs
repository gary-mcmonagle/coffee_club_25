using System;
using System.ComponentModel;
using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using ModelContextProtocol.Server;

namespace CoffeeClub.MCP.Tools;

[McpServerToolType]
public static class CoffeeTools
{
    [McpServerTool, Description("Get a list of coffees.")]
    public static async Task<CoffeeDto[]> GetCoffees(ICoffeeService coffeeService)
    {
        var coffees = await coffeeService.GetCoffeesAsync();
        return coffees.ToArray();
    }

    [McpServerTool, Description("Add a new coffee.")]
    public static async Task<CoffeeDto> AddCoffee(ICoffeeService coffeeService, CreateCoffeeDto coffeeDto)
    {
        var coffee = await coffeeService.AddCoffeeAsync(coffeeDto);
        return coffee;
    }

    [McpServerTool, Description("Get a coffee by ID.")]
    public static async Task<CoffeeDto?> GetCoffee(ICoffeeService coffeeService, Guid id)
    {
        var coffee = await coffeeService.GetCoffeeAsync(id);
        return coffee;
    }

    [McpServerTool, Description("Delete a coffee by ID.")]
    public static async Task<bool> DeleteCoffee(ICoffeeService coffeeService, Guid id)
    {
        var result = await coffeeService.DeleteCoffeeAsync(id);
        return result;
    }
}
