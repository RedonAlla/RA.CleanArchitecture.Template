using System;
using System.Reflection;

namespace VsClArch.Template.Integration;

/// <summary>
/// Helper class to get a reference to the Integration assembly.
/// </summary>
public static class Integration
{
    /// <summary>
    /// Gets the assembly containing the Integration types.
    /// </summary>
    public static readonly Assembly Assembly = typeof(Integration).Assembly;
}
