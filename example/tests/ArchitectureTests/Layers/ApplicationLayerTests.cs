using NetArchTest.Rules;
using Shouldly;
using Xunit;

namespace ArchitectureTests.Layers;

/// <summary>
/// Contains architecture tests for the Application layer.
/// </summary>
public class ApplicationLayerTests : BaseTest
{
    /// <summary>
    /// Verifies that the Application layer does not have dependencies on forbidden layers.
    /// <list type="bullet">
    /// <listheader>
    ///     <term>Forbidden Layers</term>
    ///     <description>Application layer should not have any dependency on fallowing layers:</description>
    /// </listheader>
    /// <item>
    ///     <description>Infrastructure</description>
    /// </item>
    /// <item>
    ///     <description>Integration</description>
    /// </item>
    /// <item>
    ///     <description>Persistence</description>
    /// </item>
    /// <item>
    ///     <description>Api</description>
    /// </item>
    /// <item>
    ///     <description>Api</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Application should depend only on Domain layer.
    /// </remarks>
    [Fact]
    public void Application_Should_Not_DependOn_ForbiddenLayers()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                InfrastructureNamespace,
                IntegrationNamespace,
                PersistenceNamespace,
                ApiNamespace,
                ApiContractsNamespace
            )
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("Application should not depend on forbidden layers");
    }
}
