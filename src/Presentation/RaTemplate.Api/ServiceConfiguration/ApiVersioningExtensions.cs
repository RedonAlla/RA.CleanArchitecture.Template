using System;
using Asp.Versioning;

namespace RaTemplate.Api.ServiceConfiguration;

internal static class ApiVersioningExtensions
{
    extension(IServiceCollection services)
    {
        public IApiVersioningBuilder AddApiVersioningWithExplorer()
        {
            return services
                .AddApiVersioning(options =>
                {
                    //options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Api-Version")
                    );
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
