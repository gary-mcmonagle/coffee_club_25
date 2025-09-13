
#pragma warning disable ASPIRECOSMOSDB001
using Azure.Provisioning.CosmosDB;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);



var isDev = !builder.ExecutionContext.IsPublishMode;
var insights = builder.AddAzureApplicationInsights("MyApplicationInsights");

var serviceBus = builder.AddAzureServiceBus("messaging");
if (isDev)
{
    serviceBus = serviceBus.RunAsEmulator(
        emulator =>
        {
            emulator.WithHostPort(7777);
        });
}

var queue = serviceBus.AddServiceBusQueue("coffee-queue");

var cosmos = builder.AddAzureCosmosDB("cosmosdb");
if (isDev)
{
    cosmos = cosmos.RunAsPreviewEmulator(
        emulator =>
        {
            emulator.WithDataVolume();
            emulator.WithDataExplorer();
        }).WithEndpoint(8081, 8081); // Set a fixed port
}
else
{
    cosmos = cosmos.ConfigureInfrastructure(
        infra =>
        {
            var cosmosDbAccount = infra.GetProvisionableResources()
                                   .OfType<CosmosDBAccount>()
                                   .Single();

            cosmosDbAccount.Kind = CosmosDBAccountKind.GlobalDocumentDB;
        });
}

var customers = cosmos.AddCosmosDatabase("coffeeclubdb");
var coffees = customers.AddContainer("coffees", "/id");

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator();

var functions = builder.AddAzureFunctionsProject<Projects.CoffeeClub_Functions>("functions")
                       .WithHostStorage(storage)
                       .WithReference(coffees)
                       .WaitFor(coffees);


var coreApi = builder.AddProject<Projects.CoffeeClub_Core>("coreapi")
    .WithHttpHealthCheck("/health")
    .WaitFor(coffees)
    .WithReference(coffees)
    .WaitFor(serviceBus)
    .WithReference(serviceBus);

if (!isDev && insights != null)
{
    coreApi = coreApi.WaitFor(insights).WithReference(insights);
}

var mcpApi = builder.AddProject<Projects.CoffeeClub_MCP>("mcpapi")
    .WithHttpHealthCheck("/health")
    .WithReference(coreApi)
    .WaitFor(coreApi)
    .WithExternalHttpEndpoints();
if (!isDev && insights != null)
{
    mcpApi = mcpApi.WithReference(insights);
}


var bffApi = builder.AddProject<Projects.CoffeeClub_BFF>("bffapi")
    .WithHttpHealthCheck("/health")
    .WithReference(coreApi)
    .WaitFor(coreApi);
if (!isDev && insights != null)
{
    bffApi = bffApi.WithReference(insights);
}


var uiApi = builder.AddProject<Projects.CoffeeClub_UI>("ui")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(bffApi)
    .WaitFor(bffApi);
if (!isDev && insights != null)
{
    uiApi = uiApi.WithReference(insights);
}


builder.Build().Run();

