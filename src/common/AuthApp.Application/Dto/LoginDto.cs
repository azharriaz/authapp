namespace AuthApp.Application.Dto;

public class LoginDto
{
    public ApplicationUserDto User { get; set; }

    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
