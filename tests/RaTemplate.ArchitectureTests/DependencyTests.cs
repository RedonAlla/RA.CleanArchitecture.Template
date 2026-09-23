using NetArchTest.Rules;
using Xunit;

namespace RaTemplate.ArchitectureTests;

/// <summary>
/// Contains architecture tests to verify project dependencies and layer isolation.
/// </summary>
public class DependencyTests
{
    /// <summary>
    /// Verifies that the API Contracts layer does not depend on any other projects in the solution.
    /// </summary>
    [Fact]
    public void ApiContracts_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
            Namespaces.Domain,
            Namespaces.Application,
            Namespaces.Infrastructure,
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
            .InAssembly(Assemblies.ApiContracts)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }

    /// <summary>
    /// Verifies that the Domain layer does not depend on any other projects in the solution.
    /// </summary>
    [Fact]
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

    /// <summary>
    /// Verifies that the Application layer does not depend on forbidden projects in the solution.
    /// </summary>
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
            Namespaces.Api,
            Namespaces.ApiContracts,
            Namespaces.Infrastructure,
#if UseIntegrations
            Namespaces.Integration,
#endif
#if UseAnyDatabase
            Namespaces.Persistence,
#endif
        ];

        // Act
        TestResult testResult = Types
            .InAssembly(Assemblies.Application)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }

    /// <summary>
    /// Verifies that the Infrastructure layer does not depend on forbidden projects in the solution.
    /// </summary>
    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
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
            .InAssembly(Assemblies.Infrastructure)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }

#if UseAnyDatabase
    /// <summary>
    /// Verifies that the Persistence layer does not depend on forbidden projects in the solution.
    /// </summary>
    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
            Namespaces.Api,
            Namespaces.ApiContracts,
            Namespaces.Infrastructure,
#if UseIntegrations
            Namespaces.Integration,
#endif
        ];

        // Act
        TestResult testResult = Types
            .InAssembly(Assemblies.Persistence)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }
#endif
#if UseIntegrations
    /// <summary>
    /// Verifies that the Integration layer does not depend on forbidden projects in the solution.
    /// </summary>
    [Fact]
    public void Integration_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange

        string[] otherProjects =
        [
            Namespaces.Api,
            Namespaces.ApiContracts,
            Namespaces.Infrastructure,
#if UseAnyDatabase
            Namespaces.Persistence,
#endif
        ];

        // Act
        TestResult testResult = Types
            .InAssembly(Assemblies.Integration)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }
#endif
}

