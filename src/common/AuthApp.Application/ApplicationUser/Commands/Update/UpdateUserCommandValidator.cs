using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationUser.Commands.UpdateUser;


public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IStringLocalizer<CreateUserCommandValidator> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["user.id.required"]);
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(50).When(x => x.Username != null);
        RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null).WithMessage(localizer["email.required"]);
    }
}
