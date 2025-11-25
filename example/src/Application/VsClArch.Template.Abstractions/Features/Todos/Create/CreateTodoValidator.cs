using FluentValidation;
using VsClArch.Template.Application.Todos.Create;

namespace VsClArch.Template.Application.Features.Todos.Create;

/// <summary>
/// Validator for <see cref="CreateTodoInput"/>.
/// </summary>
internal sealed class CreateTodoValidator : AbstractValidator<CreateTodoInput>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoValidator"/> class.
    /// </summary>
    public CreateTodoValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Description).NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(c => c.DueDate).GreaterThanOrEqualTo(DateTime.Today).When(x => x.DueDate.HasValue);
    }
}
