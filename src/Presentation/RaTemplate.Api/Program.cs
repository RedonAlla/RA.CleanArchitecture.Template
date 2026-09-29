using RA.Utilities.Logging.Core.Extensions;
using RaTemplate.Api;
//#if (UseAnyDatabase)
using RaTemplate.Persistence.Database;
//#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddLoggingWithConfiguration();

builder.Services.AddServices(builder.Configuration);

WebApplication app = builder.Build();

//#if (UseAnyDatabase)
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    await scope.InitializeDatabaseAsync();
}

//#endif
app.UsePipelines();

await app.RunAsync();
