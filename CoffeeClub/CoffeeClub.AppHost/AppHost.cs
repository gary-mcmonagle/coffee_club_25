#pragma warning disable ASPIRECOSMOSDB001

var builder = DistributedApplication.CreateBuilder(args);

//var cosmos = builder.AddAzureCosmosDB("cosmos");

var cosmos = builder.AddAzureCosmosDB("cosmos-db").RunAsPreviewEmulator(
                     emulator =>
                     {
                         emulator.WithDataVolume();


                         emulator.WithDataExplorer();
                     }).WithEndpoint(8081, 8081); // Set a fixed port

var customers = cosmos.AddCosmosDatabase("coffeeclubdb");
var coffees = customers.AddContainer("coffees", "/id");


var apiService = builder.AddProject<Projects.CoffeeClub_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(coffees)
    .WaitFor(coffees)
    .WithExternalHttpEndpoints();


var mcpApiService = builder.AddProject<Projects.CoffeeClub_MCPApi>("mcpapiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithExternalHttpEndpoints();

    

builder.AddProject<Projects.CoffeeClub_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
