using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationRole.Commands.Delete;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator(IStringLocalizer<CreateUserCommandValidator> localizer)
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(localizer["role.id.required"]);
    }
}
