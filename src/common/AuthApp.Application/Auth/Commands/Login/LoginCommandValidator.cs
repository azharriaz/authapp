using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace AuthApp.Application.Auth.Commands.Login;

public class GetTokenQueryValidator : AbstractValidator<LoginCommand>
{
    public GetTokenQueryValidator(IStringLocalizer<CreateUserCommandValidator> localizer)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(localizer["username.required"])
            .MinimumLength(3).WithMessage(localizer["username.minlength", 3])
            .MaximumLength(50).WithMessage(localizer["username.maxlength", 50])
            .Matches(@"^[a-zA-Z0-9_.-]*$").WithMessage(localizer["username.invalid_chars"])
            .Must(NotContainSqlInjectionPatterns).WithMessage(localizer["username.unsafe"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["password.required"])
            .MinimumLength(6).WithMessage(localizer["password.minlength", 6])
            .MaximumLength(100).WithMessage(localizer["password.maxlength", 100])
            .Must(NotContainSqlInjectionPatterns).WithMessage(localizer["password.unsafe"]);
    }

    private bool NotContainSqlInjectionPatterns(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        var blacklistPatterns = new[]
        {
            @"--", @"\*", @"\b(SELECT|INSERT|DELETE|DROP|UPDATE|EXEC|UNION|ALTER)\b", @"[';]", @"\bOR\b", @"\bAND\b"
        };

        foreach (var pattern in blacklistPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return false;
        }

        return true;
    }
}

