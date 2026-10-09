using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using RA.Utilities.Api.Extensions;
//#if (UseAuthorization)
using RA.Utilities.Authentication.JwtBearer.Extensions;
//#endif
using RA.Utilities.Logging.Core.Extensions;
using RaTemplate.Api.ServiceConfiguration;
using RaTemplate.Application;
using RaTemplate.Infrastructure;
//#if (UseAnyDatabase)
using RaTemplate.Persistence.Database;
//#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.AddLoggingWithConfiguration();

builder.Services
    .Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddDefaultHealthChecks()
    .AddOpenApi(configuration)
    .AddRaExceptionHandling()
    .AddProblemDetails();
//#if (UseAuthorization)
builder.Services.AddAuthorization(configuration);
//#endif
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(configuration)
    .AddDefaultMiddlewares();

WebApplication app = builder.Build();

//#if (UseAnyDatabase)
if (app.Environment.IsDevelopment())
{
    await RaTemplateDbInitializer.InitializeDatabaseAsync(app.Services);
}
//#endif
if (!app.Environment.IsProduction())
{
    app.UseOpenApi();
}

app.MapHealthCheckEndpoints()
   .UseDefaultMiddlewares();

//#if (UseAuthorization)
app.UseAuth();
//#endif
app.MapEndpoints();

await app.RunAsync();
