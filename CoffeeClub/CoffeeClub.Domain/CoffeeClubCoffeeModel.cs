namespace CoffeeClub.Domain;

public record CoffeeClubCoffeeModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;
    public string Roast { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
