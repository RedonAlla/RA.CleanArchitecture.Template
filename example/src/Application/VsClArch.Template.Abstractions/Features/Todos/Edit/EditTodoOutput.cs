namespace VsClArch.Template.Application.Todos.Edit;

/// <summary>
/// Represents the output after editing a Todo item.
/// </summary>
public sealed class EditTodoOutput
{
    /// <summary>
    /// Gets or sets the user ID associated with the Todo item.
    /// </summary>
    public Guid UserId { get; set; }
    
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

    /// <summary>
    /// Gets or sets the creation date of the Todo item.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
