using RA.Utilities.Authentication.JwtBearer.Extensions;
using RA.Utilities.Authorization.Extensions;

namespace RaTemplate.Api.Extensions;

internal static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAppUser()
            .AddJwtBearerAuthentication(configuration);

        // Here you can add your Authorization polices.

        return services;
    }
}
