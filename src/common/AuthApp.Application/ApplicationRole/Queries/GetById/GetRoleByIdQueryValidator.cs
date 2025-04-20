using AuthApp.Application.ApplicationRole.Commands.Update;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AuthApp.Application.ApplicationRole.Queries.GetById;

public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator(IStringLocalizer<UpdateRoleCommandValidator> localizer)
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(localizer["role.id.required"]);
    }
}
