using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Register application services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDataverseService, DataverseService>();
builder.Services.AddScoped<IMonkeyService, MonkeyService>();

builder.Build().Run();
