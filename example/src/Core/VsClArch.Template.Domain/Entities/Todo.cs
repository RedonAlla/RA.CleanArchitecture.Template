using RA.Utilities.Data.Entities;

namespace VsClArch.Template.Domain.Entities;

/// <summary>
/// Represents a Todo item in the domain.
/// </summary>
public sealed class Todo : BaseEntity
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
    /// Gets or sets the date when the Todo item was completed.
    /// </summary>
    public DateTime CompletedAt { get; set; }
}
