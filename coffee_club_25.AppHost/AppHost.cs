var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.coffee_club_25_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.coffee_club_25_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
