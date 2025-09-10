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

    protected List<CoffeeDto> Coffees = new();

    protected override async Task OnInitializedAsync()
    {

        Coffees = await CoffeeService.GetCoffeesAsync();
    }

    private async Task RefreshCoffeesAsync()
    {
        Coffees = await CoffeeService.GetCoffeesAsync();
        StateHasChanged();
    }

    protected async Task HandleCoffeeDeleted(Guid id)
    {
        await CoffeeService.DeleteCoffeeAsync(id);
        await RefreshCoffeesAsync();
    }

}
