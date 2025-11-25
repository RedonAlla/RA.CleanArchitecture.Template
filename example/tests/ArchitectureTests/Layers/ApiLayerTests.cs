using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;
using RA.Utilities.Api.Abstractions;
using Shouldly;
using Xunit;

namespace ArchitectureTests.Layers;

/// <summary>
/// Contains architecture tests for the API layer.
/// </summary>
public class ApiLayerTests : BaseTest
{
    /// <summary>
    /// Verifies that the API Contracts layer should not gave any Dependency
    /// </summary>
    [Fact]
    public void ApiContracts_Should_Not_HaveAnyDependency()
    {
        TestResult result = Types.InAssembly(ApiContractsAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                DomainNamespace,
                ApplicationNamespace,
                InfrastructureNamespace,
                IntegrationNamespace,
                PersistenceNamespace
                //ApiNamespace
            )
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    /// <summary>
    /// Verifies that API endpoints that accept a request body have a dependency on the API Contracts layer.
    /// This ensures that request objects are defined in the correct project.
    /// </summary>
    [Fact]
    public void Api_Should_HaveDependencyOn_ApiContracts()
    {
        TestResult result = Types.InAssembly(ApiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .And()
            .HaveCustomAttribute(typeof(FromBodyAttribute))
            .Should()
            .HaveDependencyOn(ApiContractsNamespace) // Assert they also depend on ApiContracts
            .GetResult();

        // Assert
        result.IsSuccessful.ShouldBeTrue(
            "The following endpoints have a [FromBody] parameter but do not depend on the ApiContracts layer:" +
            (result.FailingTypes?.Any() == true 
                ? $"\n{string.Join("\n", result.FailingTypes.Select(t => t.FullName))}" 
                : string.Empty));
    }
}
