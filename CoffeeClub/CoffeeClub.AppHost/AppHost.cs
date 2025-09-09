var builder = DistributedApplication.CreateBuilder(args);


var coreApi = builder.AddProject<Projects.CoffeeClub_Core>("coresapi")
    .WithHttpHealthCheck("/health");


var bffApi = builder.AddProject<Projects.CoffeeClub_BFF>("bffapi")
    .WithHttpHealthCheck("/health")
    .WithHttpEndpoint(3010, 8080, "bffapi")
    .WithReference(coreApi)
    .WaitFor(coreApi);


builder.AddProject<Projects.CoffeeClub_UI>("ui")
    .WithHttpHealthCheck("/health")
    .WithReference(bffApi)
    .WaitFor(bffApi);


builder.Build().Run();

