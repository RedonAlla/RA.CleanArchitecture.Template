using System.Reflection;

namespace RaTemplate.ArchitectureTests;

internal static class Assemblies
{
    public static readonly Assembly Domain = typeof(Domain.AssemblyReference).Assembly;
    public static readonly Assembly Application = typeof(Application.AssemblyReference).Assembly;
    public static readonly Assembly Infrastructure = typeof(Infrastructure.AssemblyReference).Assembly;
#if UseIntegrations
    public static readonly Assembly Integration = typeof(Integration.AssemblyReference).Assembly;
#endif
#if UseAnyDatabase
    public static readonly Assembly Persistence = typeof(Persistence.AssemblyReference).Assembly;
#endif
    public static readonly Assembly Api = typeof(Api.AssemblyReference).Assembly;
    public static readonly Assembly ApiContracts = typeof(Api.Contracts.AssemblyReference).Assembly;
}
