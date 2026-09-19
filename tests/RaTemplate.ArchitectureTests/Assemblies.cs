using System.Reflection;

namespace RaTemplate.ArchitectureTests;

internal static class Assemblies
{
    /// <summary>
    /// Gets the assembly for the Domain layer.
    /// </summary>
    //protected static readonly Assembly DomainAssembly = Assembly.Load("RaTemplate.Domain");
    public static readonly Assembly Domain = typeof(Domain.AssemblyReference).Assembly;

    /// <summary>
    /// Gets the assembly for the Application layer.
    /// </summary>
    public static readonly Assembly Application = typeof(Application.AssemblyReference).Assembly;

    /// <summary>
    /// Gets the assembly for the Infrastructure layer.
    /// </summary>
    public static readonly Assembly Infrastructure = typeof(Infrastructure.AssemblyReference).Assembly;

#if UseIntegrations
    /// <summary>
    /// Gets the assembly for the Integration layer.
    /// </summary>
    public static readonly Assembly Integration = typeof(Integration.AssemblyReference).Assembly;
#endif
#if UseAnyDatabase
    /// <summary>
    /// Gets the assembly for the Persistence layer.
    /// </summary>
    public static readonly Assembly Persistence = typeof(Persistence.AssemblyReference).Assembly;
#endif

    /// <summary>
    /// Gets the assembly for the API layer.
    /// </summary>
    public static readonly Assembly Api = typeof(Api.AssemblyReference).Assembly;

    /// <summary>
    /// Gets the assembly for the API Contracts layer.
    /// </summary>
    public static readonly Assembly ApiContracts = typeof(Api.Contracts.AssemblyReference).Assembly;
}
