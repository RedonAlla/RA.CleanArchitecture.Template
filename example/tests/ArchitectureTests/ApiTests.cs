using NetArchTest.Rules;
using RA.Utilities.Api.Abstractions;
using Shouldly;
using Xunit;

namespace ArchitectureTests;

/// <summary>
/// Contains architecture tests for the API layer.
/// </summary>
public class ApiTests : BaseTest
{
    /// <summary>
    /// Verifies that the API layer has a dependency on the BKT Utilities Application.
    /// </summary>
    /// <remarks>
    /// Enforcing that API endpoints use IFeature to ensure they adhere to the CQRS pattern and keep the presentation layer decoupled from the application logic.
    /// </remarks>
    [Fact]
    public void Api_Should_Use_IFeature_Pattern()
    {
        TestResult result = Types.InAssembly(ApiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .HaveDependencyOn("RA.Utilities.Api.Abstractions")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(
            result.FailingTypes?.Any() == true
                ? $"Failing types: {string.Join(", ", result.FailingTypes.Select(t => t.Name))}"
                : "The API layer should depend on RA.Utilities.Application."
        );
    }

    /// <summary>
    /// Verifies that all endpoints have a dependency on the BKT.Utilities.Application namespace.
    /// </summary>
    [Fact]
    public void Endpoints_Should_Return_IActionResult()
    {
        TestResult result = Types.InAssembly(ApiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .HaveDependencyOn("RA.Utilities.Feature.Abstractions")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
