using RA.Utilities.Logging.Core.Extensions;
using RaTemplate.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddLoggingWithConfiguration();

builder.Services.AddServices(builder.Configuration);

WebApplication app = builder.Build();

app.UsePipelines();

await app.RunAsync();
