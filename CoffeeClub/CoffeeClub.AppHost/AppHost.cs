#pragma warning disable ASPIRECOSMOSDB001

var builder = DistributedApplication.CreateBuilder(args);


var cosmos = builder.AddAzureCosmosDB("cosmos-db").RunAsPreviewEmulator(
                     emulator =>
                     {
                         emulator.WithDataVolume();
                         emulator.WithDataExplorer();
                     }).WithEndpoint(8081, 8081); // Set a fixed port

var customers = cosmos.AddCosmosDatabase("coffeeclubdb");
var coffees = customers.AddContainer("coffees", "/id");

var coreApi = builder.AddProject<Projects.CoffeeClub_Core>("coreapi")
    .WithHttpHealthCheck("/health")
    .WaitFor(coffees)
    .WithReference(coffees);


var bffApi = builder.AddProject<Projects.CoffeeClub_BFF>("bffapi")
    .WithHttpHealthCheck("/health")
    // .WithHttpEndpoint(3010, 8080, "bffapi")
    .WithReference(coreApi)
    .WaitFor(coreApi);


builder.AddProject<Projects.CoffeeClub_UI>("ui")
    .WithHttpHealthCheck("/health")
    .WithReference(bffApi)
    .WaitFor(bffApi);


builder.Build().Run();

