using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationRole.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(IStringLocalizer<CreateRoleCommandValidator> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["role.name.required"]).MaximumLength(50).WithMessage(localizer["role.name.maxlength", 50]);
        RuleFor(x => x.Description).MaximumLength(200).WithMessage(localizer["role.description.maxlength", 200]);
    }
}
