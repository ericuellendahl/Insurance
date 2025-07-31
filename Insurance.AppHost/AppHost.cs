var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Insurance_Propost>("insurance-propost");

builder.AddProject<Projects.Insurance_Hiring>("insurance-hiring");

await builder.Build().RunAsync();
