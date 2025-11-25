using System;

namespace VsClArch.Template.Api.Contracts.V1.Todos.Create;

/// <summary>
/// Represents the request payload for creating a new Todo item.
/// </summary>
public class CreateTodoRequest
{
    /// <summary>
    /// Gets or sets the description of the Todo item.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the optional due date for the Todo item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the list of labels for the Todo item.
    /// </summary>
    public List<string> Labels { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the Todo item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}
