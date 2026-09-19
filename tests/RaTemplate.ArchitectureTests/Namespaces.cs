using System.Reflection;

namespace RaTemplate.ArchitectureTests;

internal static class Namespaces
{
    public static readonly string Domain = Assemblies.Domain.GetName()?.Name!;
    public static readonly string Application = Assemblies.Application.GetName()?.Name!;
    public static readonly string Infrastructure = Assemblies.Infrastructure.GetName()?.Name!;

#if UseIntegrations
    /// <summary>
    /// Gets the assembly for the Integration layer.
    /// </summary>
    public static readonly Assembly Integration = Assemblies.Integration.GetName()?.Name!;
#endif
#if UseAnyDatabase
    /// <summary>
    /// Gets the assembly for the Persistence layer.
    /// </summary>
    public static readonly Assembly Persistence = Assemblies.Persistence.GetName()?.Name!;#endif
#endif

    public static readonly string Api = Assemblies.Api.GetName()?.Name!;
    public static readonly string ApiContracts = Assemblies.ApiContracts.GetName()?.Name!;
}
