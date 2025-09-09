namespace CoffeeClub.Domain.Dtos;

public record CreateCoffeeDto
{
    public string Name { get; init; } = string.Empty;
}
