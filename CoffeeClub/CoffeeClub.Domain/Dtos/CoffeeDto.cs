namespace CoffeeClub.Domain.Dtos;

public record CoffeeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
