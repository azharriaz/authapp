using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationRole.Commands.Update;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IStringLocalizer<UpdateRoleCommandValidator> localizer)
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(localizer["role.id.required"]);

        RuleFor(x => x.NewName)
            .NotEmpty().WithMessage(localizer["role.name.required"])
            .MaximumLength(50).WithMessage(localizer["role.name.maxlength", 50])
            .When(x => !string.IsNullOrEmpty(x.NewName));

        RuleFor(x => x.NewDescription)
            .MaximumLength(200).WithMessage(localizer["role.description.maxlength", 200])
            .When(x => !string.IsNullOrEmpty(x.NewDescription));
    }
}
