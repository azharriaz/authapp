using AuthApp.Application.Dto;

namespace AuthApp.Application.ApplicationUser.Queries.GetToken;

public class LoginResponse
{
    public ApplicationUserDto User { get; set; }

    public string Token { get; set; }
}
