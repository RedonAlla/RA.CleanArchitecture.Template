using System;

namespace VsClArch.Template.Api.Contracts.V1.Todos.Edit;

/// <summary>
/// Represents the request payload for editing an existing Todo item.
/// </summary>
public class EditTodoRequest
{
    /// <summary>
    /// Gets or sets the updated description of the Todo item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the updated optional due date for the Todo item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the updated list of labels for the Todo item.
    /// </summary>
    public List<string> Labels { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the Todo item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}
