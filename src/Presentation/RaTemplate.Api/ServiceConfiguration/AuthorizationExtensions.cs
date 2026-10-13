using RA.Utilities.Authentication.JwtBearer.Extensions;
using RA.Utilities.Authorization.Extensions;

namespace RaTemplate.Api.ServiceConfiguration;

internal static class AuthorizationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAuthorization(IConfiguration configuration)
        {
            services
                .AddUserContext()
                .AddJwtBearerAuthentication(configuration);

            // Here you can add your Authorization policies.

            return services;
        }
    }
}
