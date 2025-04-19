﻿using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Common.Models;
using AuthApp.Application.Dto;

namespace AuthApp.Application.Auth.Commands;

public class LoginCommand : IRequestWrapper<LoginDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler(IAuthService authService) : IRequestHandlerWrapper<LoginCommand, LoginDto>
{
    public async Task<ServiceResult<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var authResult = await authService.AuthenticateAsync(request);

        return authResult == default
            ? ServiceResult.Failed<LoginDto>(ServiceError.NotFound)
            : ServiceResult.Success((authResult));
    }
}