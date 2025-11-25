using System;
using System.Reflection;

namespace VsClArch.Template.Api;

/// <summary>
/// Helper class to get a reference to the API assembly.
/// </summary>
public static class Api
{
    /// <summary>
    /// Gets the assembly containing the API types.
    /// </summary>
    public static readonly Assembly Assembly = typeof(Api).Assembly;
}
