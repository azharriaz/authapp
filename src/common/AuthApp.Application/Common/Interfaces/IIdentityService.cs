using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Common.Interfaces;

public interface IIdentityService
{
    #region Users
    Task<List<ApplicationUserDto>> GetUsersAsync();
    Task<ApplicationUserDto> GetUserByIdAsync(string userId);
    Task<(Result Result, string UserId)> CreateUserAsync(CreateUserCommand input);
    Task<Result> UpdateUserAsync(string userId, string newUsername, string newEmail);
    Task<string> GetUserNameAsync(string userId);
    Task<Result> DeleteUserAsync(string userId);
    #endregion

    #region Auth
    Task<ApplicationUserDto> CheckUserPasswordAsync(string username, string password);
    Task AssignRolesToUser(string userId, IEnumerable<string> roles);
    Task<bool> UserIsInRoleAsync(string userId, string role);
    #endregion

    #region Roles
    Task<ApplicationRoleDto> GetRoleByIdAsync(string roleId);
    Task<List<ApplicationRoleDto>> GetRolesAsync();
    Task<(Result Result, string RoleId)> CreateRoleAsync(string name, string description);
    Task<Result> UpdateRoleAsync(string roleId, string newName, string newDescription);
    Task<Result> DeleteRoleAsync(string roleId);
    Task UpdateUserRoles(string userId, IEnumerable<string> newRoles);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    #endregion
}
