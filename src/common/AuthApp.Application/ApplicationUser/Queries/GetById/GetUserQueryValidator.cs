using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationUser.Queries.GetById;

public class GetUserQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    private readonly IStringLocalizer<GetUserQueryValidator> _localizer;

    public GetUserQueryValidator(IStringLocalizer<GetUserQueryValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(_localizer["user.id.required"])
            .MaximumLength(450).WithMessage(_localizer["user.id.maxlength"]);
    }
}
