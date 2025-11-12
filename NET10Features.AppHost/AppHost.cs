var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NET10Features_WebAPI>("net10features-webapi");

builder.Build().Run();
