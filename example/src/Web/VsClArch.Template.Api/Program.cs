#pragma warning disable 1591

using RA.Utilities.Logging.Core.Extensions;
using VsClArch.Template.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// var builder = new ConfigurationBuilder()
//                 .SetBasePath(env.ContentRootPath)
//                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//                 .AddEnvironmentVariables();

builder.AddLoggingWithConfiguration();

builder.Services.AddServices(builder.Configuration);

WebApplication app = builder.Build();

app.UsePipelines();

await app.RunAsync();
