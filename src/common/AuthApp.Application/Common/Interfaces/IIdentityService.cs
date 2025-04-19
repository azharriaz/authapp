using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ApplicationUserDto> GetUserByIdAsync(string userId);
    Task<List<ApplicationUserDto>> GetUsersAsync();
    Task<ApplicationRoleDto> GetRoleByIdAsync(string roleId);
    Task<List<ApplicationRoleDto>> GetRolesAsync();

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
    Task<(Result Result, string RoleId)> CreateRoleAsync(string name, string description);
    Task<Result> UpdateUserAsync(string userId, string newUsername, string newEmail);

    Task<Result> UpdateRoleAsync(string roleId, string newName, string newDescription);
    Task<Result> DeleteRoleAsync(string roleId);

    Task AssignRolesToUser(string userId, IEnumerable<string> roles);
    Task UpdateUserRoles(string userId, IEnumerable<string> newRoles);

    Task<ApplicationUserDto> CheckUserPasswordAsync(string username, string password);

    Task<bool> UserIsInRoleAsync(string userId, string role);

    Task<string> GetUserNameAsync(string userId);
}
