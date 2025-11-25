using System;
using System.ComponentModel.DataAnnotations;
using RA.Utilities.Integrations.Options;

namespace VsClArch.Template.Integration.JsonPlaceholder;

/// <summary>
/// Represents the settings for the JSON Placeholder API integration for Todos.
/// </summary>
public class TodoJsonPlaceholderSettings : BaseApiSettings<TodoJsonPlaceholderActions>
{
    /// <summary>
    /// Gets the configuration key for the JSON Placeholder settings.
    /// </summary>
    public const string JsonPlaceholderSettingsKey = "Integrations:TodoJsonPlaceholderSettings";
}

/// <summary>
/// Defines the actions/endpoints for the JSON Placeholder API.
/// </summary>
public class TodoJsonPlaceholderActions {
    /// <summary>
    /// Gets or sets the relative path for the users endpoint.
    /// </summary>
    [Required]
    public string? GetUsers { get; set; }
}
