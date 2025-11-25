using FluentValidation;
using VsClArch.Template.Application.Todos.Edit;

namespace VsClArch.Template.Application.Features.Todos.Edit;

/// <summary>
/// Validator for <see cref="EditTodoInput"/>.
/// </summary>
internal sealed class EditTodoValidator : AbstractValidator<EditTodoInput>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditTodoValidator"/> class.
    /// </summary>
    public EditTodoValidator()
    {
        RuleFor(c => c.Description).NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(c => c.DueDate).GreaterThanOrEqualTo(DateTime.Today).When(x => x.DueDate.HasValue);
    }
}
