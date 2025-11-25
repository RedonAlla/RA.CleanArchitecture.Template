using NetArchTest.Rules;
using Shouldly;
using Xunit;

namespace ArchitectureTests.Layers;

/// <summary>
/// Tests for the Domain layer.
/// </summary>
public class DomainLayerTests : BaseTest
{
    /// <summary>
    /// Verifies that the Domain layer does not have a dependency on any other layer.
    /// </summary>
    [Fact]
    public void Domain_Should_Not_HaveDependencyOn_OtherLayers()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                ApplicationNamespace,
                InfrastructureNamespace,
                IntegrationNamespace,
                PersistenceNamespace,
                ApiNamespace,
                ApiContractsNamespace
            )
            .GetResult();
        
        result.IsSuccessful.ShouldBeTrue("Domain should not depend on other Layers");
    }
}
