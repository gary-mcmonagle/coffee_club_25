using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeClub.BFF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoffeeController : ControllerBase
{
    private readonly ICoffeeService _coffeeService;

    public CoffeeController(ICoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCoffees()
    {
        var coffees = await _coffeeService.GetCoffeesAsync();
        return Ok(coffees);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCoffee(Guid id)
    {
        var coffee = await _coffeeService.GetCoffeeAsync(id);
        if (coffee == null)
        {
            return NotFound();
        }
        return Ok(coffee);
    }

    [HttpPost]
    public async Task<IActionResult> AddCoffee([FromBody] CreateCoffeeDto createCoffeeDto)
    {
        var coffee = await _coffeeService.AddCoffeeAsync(createCoffeeDto);
        return CreatedAtAction(nameof(GetCoffee), new { id = coffee.Id }, coffee);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCoffee(Guid id)
    {
        var success = await _coffeeService.DeleteCoffeeAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
