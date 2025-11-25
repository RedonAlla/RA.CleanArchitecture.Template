using System;
using NetArchTest.Rules;
using RA.Utilities.Data.Entities;
using Shouldly;
using Xunit;

namespace ArchitectureTests;

/// <summary>
/// Contains architecture tests for domain layer.
/// </summary>
public class DomainTests : BaseTest
{
    private const string EntitiesNamespace = "VsClArch.Template.Domain.Entities";

    /// <summary>
    /// Verifies that all domain entities inherit from <see cref="BaseEntity"/>.
    /// </summary>
    [Fact]
    public void All_Entities_Should_Inherit_From_BaseEntity()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace(EntitiesNamespace) // adjust if your entities live elsewhere
            .Should()
            .Inherit(typeof(BaseEntity))
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("All entities must inherit from BaseEntity.");
    }

    /// <summary>
    /// Verifies that only types residing in the entities namespace inherit from <see cref="BaseEntity"/>.
    /// </summary>
    [Fact]
    public void Only_Entities_Should_Inherit_From_BaseEntity()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(BaseEntity))
            .Should()
            .ResideInNamespace(EntitiesNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("All entities must inherit from BaseEntity.");
    }

    /// <summary>
    /// Verifies that all domain entities are sealed.
    /// </summary>
    [Fact]
    public void Entities_Should_Be_Sealed()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(BaseEntity))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
