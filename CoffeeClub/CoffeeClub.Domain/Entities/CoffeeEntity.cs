using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CoffeeClub.Domain.Entities;

public record CoffeeEntity
{
    [JsonPropertyName("id")]
    [JsonProperty("id")]
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Roast { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
