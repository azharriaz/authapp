using AuthApp.Application.Auth.Commands;
using AuthApp.Application.Dto;

namespace AuthApp.Application.Common.Interfaces;

public interface IAuthService
{
    Task<LoginDto> AuthenticateAsync(LoginCommand loginCommand);
}
