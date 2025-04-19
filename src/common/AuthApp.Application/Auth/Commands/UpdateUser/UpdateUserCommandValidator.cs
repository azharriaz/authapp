using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.Application.Auth.Commands.UpdateUser;


public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(50).When(x => x.Username != null);
        RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null);
    }
}
