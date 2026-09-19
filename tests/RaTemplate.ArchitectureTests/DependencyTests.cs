using NetArchTest.Rules;
using Xunit;

namespace RaTemplate.ArchitectureTests;

[Fact]
public class DependencyTests
{
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
            Namespaces.Application,
            Namespaces.Infrastructure,
            Namespaces.Api,
            Namespaces.ApiContracts,
#if UseIntegrations
            Namespaces.Integration,
#endif
#if UseAnyDatabase
            Namespaces.Persistence,
#endif
        ];

        // Act
        TestResult testResult = Types
            .InAssembly(Assemblies.Domain)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }
}

