using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;

namespace CoffeeClub.UI.Services;

public class CoffeeService(HttpClient httpClient) : ICoffeeService
{

    private const string BaseUrl = "/api/coffee";

    public async Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto createCoffeeDto)
    {
        var response = await httpClient.PostAsJsonAsync(BaseUrl, createCoffeeDto);
        response.EnsureSuccessStatusCode();
        var coffee = await response.Content.ReadFromJsonAsync<CoffeeDto>();
        return coffee!;
    }

    public async Task<bool> DeleteCoffeeAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<CoffeeDto?> GetCoffeeAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CoffeeDto>();
    }

    public async Task<List<CoffeeDto>> GetCoffeesAsync()
    {
        var response = await httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();
        var coffees = await response.Content.ReadFromJsonAsync<List<CoffeeDto>>();
        return coffees ?? new List<CoffeeDto>();
    }
}
