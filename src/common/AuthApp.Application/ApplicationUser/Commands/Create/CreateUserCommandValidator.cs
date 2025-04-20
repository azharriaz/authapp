using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationUser.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IStringLocalizer<CreateUserCommandValidator> localizer)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(localizer["username.required"])
            .MinimumLength(3).WithMessage(localizer["username.minlength", 3]);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["firstname.required"]);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["lastname.required"]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["email.required"])
            .EmailAddress().WithMessage(localizer["email.invalid"]);

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage(localizer["roles.required"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["password.required"])
            .MinimumLength(8).WithMessage(localizer["password.minlength", 8])
            .Matches("[A-Z]").WithMessage(localizer["password.uppercase"])
            .Matches("[a-z]").WithMessage(localizer["password.lowercase"])
            .Matches("[0-9]").WithMessage(localizer["password.digit"]);
    }
}
