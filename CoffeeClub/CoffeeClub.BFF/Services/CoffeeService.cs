using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;

namespace CoffeeClub.BFF.Services;

public class CoffeeService(HttpClient httpClient) : ICoffeeService
{
    private const string BaseUrl = "api/coffee";

    public async Task<List<CoffeeDto>> GetCoffeesAsync()
    {
        var response = await httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CoffeeDto>>() ?? [];
    }

    public async Task<CoffeeDto?> GetCoffeeAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CoffeeDto>();
    }

    public async Task<CoffeeDto> AddCoffeeAsync(CreateCoffeeDto createCoffeeDto)
    {
        var response = await httpClient.PostAsJsonAsync(BaseUrl, createCoffeeDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CoffeeDto>() ?? throw new Exception("Failed to deserialize CoffeeDto");
    }

    public async Task<bool> DeleteCoffeeAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        response.EnsureSuccessStatusCode();
        return true;
    }
}
