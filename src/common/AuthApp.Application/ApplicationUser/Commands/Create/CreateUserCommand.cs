﻿using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.ApplicationUser.Commands.CreateUser;

public class CreateUserCommand : IRequestWrapper<ApplicationUserDto>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class CreateUserCommandHandler(IIdentityService identityService) : IRequestHandlerWrapper<CreateUserCommand, ApplicationUserDto>
{
    public async Task<ServiceResult<ApplicationUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var (result, userId) = await identityService.CreateUserAsync(request);

        if (!result.Succeeded)
        {
            return ServiceResult.Failed<ApplicationUserDto>(ServiceError.CustomMessage(result.Errors?.ToString() ?? string.Empty));
        }

        await identityService.AssignRolesToUserAsync(userId, request.Roles);

        return ServiceResult.Success(new ApplicationUserDto
        {
            Id = userId,
            UserName = request.Username,
            Email = request.Email,
            Roles = request.Roles
        });
    }
}
