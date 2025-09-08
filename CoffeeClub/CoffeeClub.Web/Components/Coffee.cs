namespace CoffeeClub.Web.Components;

public class Coffee
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;
    public string Roast { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
