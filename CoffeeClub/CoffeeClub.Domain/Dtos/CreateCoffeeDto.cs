namespace CoffeeClub.Domain.Dtos;

public record CreateCoffeeDto
{
    public string Name { get; set; } = string.Empty;
    public string Roast { get; set; } = string.Empty;
}
