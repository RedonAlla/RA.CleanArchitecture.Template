using System;
using System.Reflection;

namespace VsClArch.Template.Api.Contracts;

/// <summary>
/// A marker class for the Api.Contracts assembly.
/// </summary>
public static class ApiContracts
{
    /// <summary>
    /// Gets the assembly containing the API Contracts types.
    /// </summary>
    public static readonly Assembly Assembly = typeof(ApiContracts).Assembly;
}
