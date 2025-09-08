using System.Net.Http.Json;

namespace CoffeeClub.Web.Components;

public class CoffeeApiClient
{
    private readonly HttpClient _httpClient;

    public CoffeeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Coffee>> GetCoffeesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Coffee>>("/coffees") ?? new();
    }

    public async Task<Coffee?> GetCoffeeByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Coffee>($"/coffees/{id}");
    }

    public async Task<Coffee?> AddCoffeeAsync(Coffee coffee)
    {
        var response = await _httpClient.PostAsJsonAsync("/coffees", coffee);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Coffee>();
        }
        return null;
    }

    public async Task<bool> UpdateCoffeeAsync(string id, Coffee coffee)
    {
        var response = await _httpClient.PutAsJsonAsync($"/coffees/{id}", coffee);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCoffeeAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"/coffees/{id}");
        return response.IsSuccessStatusCode;
    }
}
