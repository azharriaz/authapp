using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;

namespace AuthApp.Application.Auth.Queries.GetUser;

public class GetUserQuery : IRequestWrapper<ApplicationUserDto>
{
    public string UserId { get; set; }
}
