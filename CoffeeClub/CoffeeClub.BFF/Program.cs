using CoffeeClub.BFF.Services;
using CoffeeClub.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddHttpClient<ICoffeeService, CoffeeService>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://coreapi");
    //client.BaseAddress = new("yuck");
});

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
