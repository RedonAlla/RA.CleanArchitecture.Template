using System.Reflection;

namespace RaTemplate.Domain;

/// <summary>
/// Helper class to get a reference to the BKT.Utilities.Data.Core assembly.
/// </summary>
public static class Domain
{
    /// <summary>
    /// Gets the assembly containing the BKT.Utilities.Data.Core types.
    /// </summary>
    public static readonly Assembly Assembly = typeof(Domain).Assembly;
}
