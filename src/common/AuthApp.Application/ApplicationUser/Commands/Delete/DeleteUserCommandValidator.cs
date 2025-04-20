using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationUser.Commands.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator(IStringLocalizer<CreateUserCommandValidator> localizer)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(localizer["user.id.required"]);
    }
}
