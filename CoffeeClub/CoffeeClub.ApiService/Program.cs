using CoffeeClub.ApiService.Infrastructure;
using CoffeeClub.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.AddSqlServerDbContext<CoffeeClubDbContext>(connectionName: "coffeeclubdb");

builder.AddCosmosDbContext<CoffeeClubDbContext>("cosmosdb", "coffeeclubdb", options =>
{

    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", async (ICoffeeService service) =>
{
    await service.AddCoffeeAsync(new CoffeeClubCoffeeModel
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Ethiopian Yirgacheffe",
        Origin = "Ethiopia",
        Roast = "Light",
        CreatedAt = DateTime.UtcNow
    });
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// CRUD endpoints for Coffee
app.MapGet("/coffees", async (ICoffeeService service) =>
    Results.Ok(await service.GetCoffeesAsync()));

app.MapGet("/coffees/{id}", async (string id, ICoffeeService service) =>
    await service.GetCoffeeByIdAsync(id) is CoffeeClubCoffeeModel coffee
        ? Results.Ok(coffee)
        : Results.NotFound());

app.MapPost("/coffees", async (CoffeeClubCoffeeModel coffee, ICoffeeService service) =>
{
    await service.AddCoffeeAsync(coffee);
    return Results.Created($"/coffees/{coffee.Id}", coffee);
});

app.MapPut("/coffees/{id}", async (string id, CoffeeClubCoffeeModel coffee, ICoffeeService service) =>
{
    if (id != coffee.Id) return Results.BadRequest();
    await service.UpdateCoffeeAsync(coffee);
    return Results.NoContent();
});

app.MapDelete("/coffees/{id}", async (string id, ICoffeeService service) =>
{
    await service.DeleteCoffeeAsync(id);
    return Results.NoContent();
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
