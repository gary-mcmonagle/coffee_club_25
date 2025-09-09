using System.Net.Http.Json;
using CoffeeClub.Domain;

namespace CoffeeClub.Web.Components;

public class CoffeeApiClient
{
    private readonly HttpClient _httpClient;

    public CoffeeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CoffeeClubCoffeeModel>> GetCoffeesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CoffeeClubCoffeeModel>>("/coffees") ?? new();
    }

    public async Task<CoffeeClubCoffeeModel?> GetCoffeeByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<CoffeeClubCoffeeModel>($"/coffees/{id}");
    }

    public async Task<CoffeeClubCoffeeModel?> AddCoffeeAsync(CoffeeClubCoffeeModel coffee)
    {
        var response = await _httpClient.PostAsJsonAsync("/coffees", coffee);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CoffeeClubCoffeeModel>();
        }
        return null;
    }

    public async Task<bool> UpdateCoffeeAsync(string id, CoffeeClubCoffeeModel coffee)
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
