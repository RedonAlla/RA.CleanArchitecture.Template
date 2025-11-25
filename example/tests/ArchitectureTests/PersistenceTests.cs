using NetArchTest.Rules;
using RA.Utilities.Data.Abstractions;
using RA.Utilities.Data.EntityFramework;
using Shouldly;
using Xunit;

namespace ArchitectureTests;

/// <summary>
/// Contains architecture tests for the Persistence layer.
/// </summary>
public class PersistenceTests : BaseTest
{
    private const string RepositoryPostfix = "Repository";
    private const string RepositoriesNamespace = "VsClArch\\.Template\\.Persistence\\..*";

    /// <summary>
    /// Verifies that all repository classes end with the "Repository" postfix.
    /// </summary>
    [Fact]
    public void Repositories_Should_EndWith_Repository()
    {
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .That()
            .ImplementInterface(typeof(IReadRepositoryBase<>))
            .Or()
            .ImplementInterface(typeof(IWriteRepositoryBase<>))
            .Or()
            .ImplementInterface(typeof(IRepositoryBase<>))
            .Should()
            .HaveNameEndingWith(RepositoryPostfix)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("All repository classes should be named ending with 'Repository'.");
    }

    /// <summary>
    /// Verifies that all repository classes inherit from a base repository class.
    /// </summary>
    [Fact]
    public void All_Repositories_Should_Inherit_From_BaseRepository()
    {
        /*
            (for example MyApp.Infrastructure.Persistence.Users, MyApp.Infrastructure.Persistence.Orders, etc.),
            you’ll want the rule to check all sub-namespaces under Persistence, not just a single Repositories folder.

            .ResideInNamespaceMatching("MyApp\\.Infrastructure\\.Persistence\\..*") // all sub-namespaces under Persistence
        */

        TestResult result = Types.InAssembly(PersistenceAssembly)
            .That()
            .ResideInNamespace(RepositoriesNamespace) // adjust to your repo namespace
            .Should()
            .Inherit(typeof(RepositoryBase<>))
            .Or()
            .Inherit(typeof(ReadRepositoryBase<>))
            .Or()
            .Inherit(typeof(WriteRepositoryBase<>))
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("All repositories must inherit from RepositoryBase or ReadRepositoryBase, WriteRepositoryBase.");
    }

    /// <summary>
    /// Verifies that only repository classes inherit from a base repository class.
    /// </summary>
    [Fact]
    public void Only_Repositories_Should_Inherit_From_BaseRepository()
    {
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .That()
            .Inherit(typeof(RepositoryBase<>))
            .Or()
            .Inherit(typeof(ReadRepositoryBase<>))
            .Or()
            .Inherit(typeof(WriteRepositoryBase<>))
            .Should()
            .ResideInNamespaceMatching(RepositoriesNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("Only classes inside Persistence.* namespaces should inherit from RepositoryBase or ReadRepositoryBase, WriteRepositoryBase.");
    }

    /// <summary>
    /// Verifies that all repository classes implement at least one of the IReadRepositoryBase, IWriteRepositoryBase, or IRepositoryBase interfaces.
    /// </summary>
    [Fact]
    public void All_Repositories_Should_Implement_IRepository()
    {
        TestResult result = Types.InAssembly(PersistenceAssembly)
            .That()
            .ResideInNamespaceMatching(RepositoriesNamespace)
            .And()
            .HaveNameEndingWith("Repository")
            .Should()
            .ImplementInterface(typeof(IReadRepositoryBase<>))
            .Or()
            .ImplementInterface(typeof(IWriteRepositoryBase<>))
            .Or()
            .ImplementInterface(typeof(IRepositoryBase<>))
            .GetResult();

        result.IsSuccessful.ShouldBeTrue("All repositories should implement IReadRepositoryBase or IWriteRepositoryBase, or IRepositoryBase");
    }
}
