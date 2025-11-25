namespace VsClArch.Template.Application.Todos.GetById;

/// <summary>
/// Represents the output for a single Todo item retrieved by its ID.
/// </summary>
public sealed class GetTodosByIdOutput
{
    /// <summary>
    /// Gets or sets the unique identifier for the Todo item.
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Gets or sets the date when the Todo item was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the author of the Todo item.
    /// </summary>
    public Author? Author { get; set;  }
}

/// <summary>
/// Represents the author of a Todo item.
/// </summary>
public sealed class Author
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// </summary>
    public Author()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// </summary>
    /// <param name="name">The name of the author.</param>
    /// <param name="username">The username of the author.</param>
    /// <param name="email">The email of the author.</param>
    public Author(string name, string username, string email)
    {
        Name = name;
        Username = username;
        Email = email;
    }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string? Email { get; set; }
}
