using System.Reflection;

namespace RaTemplate.ArchitectureTests;

internal static class Namespaces
{
    public static readonly string Domain = Assemblies.Domain.GetName()?.Name!;
    public static readonly string Application = Assemblies.Application.GetName()?.Name!;
    public static readonly string Infrastructure = Assemblies.Infrastructure.GetName()?.Name!;

#if UseIntegrations
    public static readonly string Integration = Assemblies.Integration.GetName()?.Name!;
#endif
#if UseAnyDatabase
    public static readonly string Persistence = Assemblies.Persistence.GetName()?.Name!;
#endif

    public static readonly string Api = Assemblies.Api.GetName()?.Name!;
    public static readonly string ApiContracts = Assemblies.ApiContracts.GetName()?.Name!;
}
