using NetArchTest.Rules;
using Shouldly;
using Xunit;

namespace ArchitectureTests.Layers;

/// <summary>
/// Contains architecture tests for the Infrastructure layer.
/// </summary>
public class InfrastructureLayerTests : BaseTest
{
    /// <summary>
    /// Verifies that the Infrastructure layer does not have a dependency on forbidden layers..
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>
    ///     <term>Forbidden Layers</term>
    ///     <description>Infrastructure layer should not have any dependency on fallowing layers:</description>
    /// </listheader>
    /// <item>
    ///     <description>Api</description>
    /// </item>
    /// <item>
    ///     <description>Api.Contracts</description>
    /// </item>
    /// <item>
    ///     <description>Domain</description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Infrastructure_Should_Not_Have_Dependency_On_ForbiddenLayer()
    {
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                ApiNamespace,
                ApiContractsNamespace,
                DomainNamespace
            )
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    /// <summary>
    /// Verifies that the Persistence layer does not have a dependency on forbidden layers..
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>
    ///     <term>Forbidden Layers</term>
    ///     <description>Persistence layer should not have any dependency on fallowing layers:</description>
    /// </listheader>
    /// <item>
    ///     <description>Api</description>
    /// </item>
    /// <item>
    ///     <description>Api.Contracts</description>
    /// </item>
    /// <item>
    ///     <description>Domain</description>
    /// </item>
    /// <item>
    ///     <description>Integration</description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Persistence_Should_Not_Have_Dependency_On_ForbiddenLayer()
    {
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                ApiNamespace,
                ApiContractsNamespace,
                //DomainNamespace,
                IntegrationNamespace
            )
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    /// <summary>
    /// Verifies that the Persistence layer does not have a dependency on forbidden layers..
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>
    ///     <term>Forbidden Layers</term>
    ///     <description>Persistence layer should not have any dependency on fallowing layers:</description>
    /// </listheader>
    /// <item>
    ///     <description>Api</description>
    /// </item>
    /// <item>
    ///     <description>Api.Contracts</description>
    /// </item>
    /// <item>
    ///     <description>Domain</description>
    /// </item>
    /// <item>
    ///     <description>Persistence</description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Integration_Should_Not_Have_Dependency_On_ForbiddenLayer()
    {
        TestResult result = Types.InAssembly(IntegrationAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                ApiNamespace,
                ApiContractsNamespace,
                DomainNamespace,
                PersistenceNamespace
            )
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
