using System.Text.Json;
using CoffeeClub.Core.Services;
using CoffeeClub.Domain.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureServiceBusClient(connectionName: "messaging");

// Add services to the container.

// Add service defaults & Aspire client integrations.

builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.AddServiceDefaults();

// builder.AddCosmosDbContext<CoffeeContext>("cosmosdb", "coffeeclubdb", options =>
// {


// });

builder.AddAzureCosmosClient(connectionName: "cosmosdb", configureClientOptions: options =>
{
    options.SerializerOptions = new CosmosSerializationOptions
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    };
});
builder.AddAzureCosmosDatabase(connectionName: "coffeeclubdb");
builder.AddAzureCosmosContainer(connectionName: "coffees");

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

app.MapControllers();
app.MapDefaultEndpoints();


app.Run();
