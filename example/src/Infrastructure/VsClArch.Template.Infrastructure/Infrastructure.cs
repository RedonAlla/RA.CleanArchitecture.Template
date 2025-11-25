using System;
using System.Reflection;

namespace VsClArch.Template.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure services in the dependency injection container.
/// </summary>
public static class Infrastructure
{
    /// <summary>
    /// Gets the assembly containing the Infrastructure types.
    /// </summary>
    public static readonly Assembly Assembly = typeof(Infrastructure).Assembly;
}
