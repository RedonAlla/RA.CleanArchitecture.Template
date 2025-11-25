using System.Reflection;
using VsClArch.Template.Api.Contracts;
using VsClArch.Template.Application;
using VsClArch.Template.Infrastructure;
using VsClArch.Template.Integration;
using VsClArch.Template.Persistence;

namespace ArchitectureTests;

/// <summary>
/// Base class for architecture tests, providing common assembly references.
/// </summary>
public abstract class BaseTest
{
    #region Assemblies

    /// <summary>
    /// Gets the assembly for the Domain layer.
    /// </summary>
    protected static readonly Assembly DomainAssembly = typeof(VsClArch.Template.Domain.Domain).Assembly;

    /// <summary>
    /// Gets the assembly for the Application layer.
    /// </summary>
    protected static readonly Assembly ApplicationAssembly = typeof(ApplicationServiceRegistration).Assembly;

    /// <summary>
    /// Gets the assembly for the Infrastructure layer.
    /// </summary>
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure).Assembly;

    /// <summary>
    /// Gets the assembly for the Integration layer.
    /// </summary>
    protected static readonly Assembly IntegrationAssembly = typeof(IntegrationServiceRegistration).Assembly;

    /// <summary>
    /// Gets the assembly for the Persistence layer.
    /// </summary>
    protected static readonly Assembly PersistenceAssembly = typeof(PersistenceDependencyInjection).Assembly;

    /// <summary>
    /// Gets the assembly for the API layer.
    /// </summary>
    protected static readonly Assembly ApiAssembly = typeof(Program).Assembly;

    /// <summary>
    /// Gets the assembly for the API Contracts layer.
    /// </summary>
    protected static readonly Assembly ApiContractsAssembly = typeof(ApiContracts).Assembly;

    #endregion Assemblies

    #region Namespaces
    /// <summary>
    /// Gets the namespace for the Domain layer.
    /// </summary>
    protected static readonly string DomainNamespace = DomainAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the Application layer.
    /// </summary>
    protected static readonly string ApplicationNamespace = ApplicationAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the Infrastructure layer.
    /// </summary>
    protected static readonly string InfrastructureNamespace = InfrastructureAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the Integration layer.
    /// </summary>
    protected static readonly string IntegrationNamespace = IntegrationAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the Persistence layer.
    /// </summary>
    protected static readonly string PersistenceNamespace = PersistenceAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the API layer.
    /// </summary>
    protected static readonly string ApiNamespace = ApiAssembly?.GetName()?.Name!;

    /// <summary>
    /// Gets the namespace for the API Contracts layer.
    /// </summary>
    protected static readonly string ApiContractsNamespace = ApiContractsAssembly?.GetName()?.Name!;

    #endregion Namespaces
}
