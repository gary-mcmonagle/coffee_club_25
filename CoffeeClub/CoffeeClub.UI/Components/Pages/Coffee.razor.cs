using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeClub.Domain.Dtos;
using CoffeeClub.Domain.Services;
using Microsoft.AspNetCore.Components;

namespace CoffeeClub.UI.Components.Pages;

public partial class Coffee : ComponentBase
{

    [Inject]
    private ICoffeeService CoffeeService { get; set; } = default!;

    protected CreateCoffeeDto newCoffee ; 

    protected List<CoffeeDto> Coffees = new();

    protected override async Task OnInitializedAsync()
    {
                newCoffee = new CreateCoffeeDto();

        Coffees = await CoffeeService.GetCoffeesAsync();
    }

    private async Task RefreshCoffeesAsync()
    {
        Coffees = await CoffeeService.GetCoffeesAsync();
        StateHasChanged();
    }

    private async Task AddCoffeeAsync(CreateCoffeeDto createCoffeeDto)
    {
        if (createCoffeeDto == null || string.IsNullOrWhiteSpace(createCoffeeDto.Name) || string.IsNullOrWhiteSpace(createCoffeeDto.Roast))
            return;

        await CoffeeService.AddCoffeeAsync(createCoffeeDto);
        await RefreshCoffeesAsync();
    }
    protected async Task DeleteCoffeeAsync(Guid id)
    {
        await CoffeeService.DeleteCoffeeAsync(id);
        await RefreshCoffeesAsync();
    }
        protected async Task HandleAddCoffee()
    {
        await AddCoffeeAsync(newCoffee);
        await RefreshCoffeesAsync();
        newCoffee = new();
    }

}
