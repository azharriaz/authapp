using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Exceptions;
using AuthApp.Domain.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager) : IIdentityService
{
    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            throw new UnauthorizeException();
        }

        return user.UserName;
    }

    public async Task<ApplicationUserDto> CheckUserPasswordAsync(string username, string password)
    {
        ApplicationUser user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (user is not null && await userManager.CheckPasswordAsync(user, password))
        {
            return mapper.Map<ApplicationUserDto>(user);
        }

        return null;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(CreateUserCommand input)
    {
        var user = new ApplicationUser
        {
            UserName = input.Username,
            Email = input.Email,
            Name = input.FirstName,
            LastName = input.LastName,
            Gsm = string.Empty
        };

        var result = await userManager.CreateAsync(user, input.Password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> UserIsInRoleAsync(string userId, string role)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            return await DeleteUserAsync(user);
        }

        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<ApplicationUserDto> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? null : mapper.Map<ApplicationUserDto>(user);
    }

    public async Task<List<ApplicationUserDto>> GetUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        return mapper.Map<List<ApplicationUserDto>>(users);
    }

    public async Task<ApplicationRoleDto> GetRoleByIdAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        return role is null ? null : mapper.Map<ApplicationRoleDto>(role);
    }

    public async Task<List<ApplicationRoleDto>> GetRolesAsync()
    {
        var roles = await roleManager.Roles.ToListAsync();
        return mapper.Map<List<ApplicationRoleDto>>(roles);
    }

    /// <summary>
    /// Creates a new role in application database.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public async Task<(Result Result, string RoleId)> CreateRoleAsync(string name, string description)
    {
        var role = new IdentityRole(name)
        {
            NormalizedName = name.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await roleManager.CreateAsync(role);

        if (result.Succeeded && !string.IsNullOrEmpty(description))
        {
            await roleManager.SetRoleNameAsync(role, description);
        }

        return (result.ToApplicationResult(), role.Id);
    }

    /// <summary>
    /// Updates the role in application.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="newName"></param>
    /// <param name="newDescription"></param>
    /// <returns></returns>
    public async Task<Result> UpdateRoleAsync(string roleId, string newName, string newDescription)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role is null) return Result.Failure(new[] { "Role not found" });

        role.Name = newName;
        role.NormalizedName = newName.ToUpper();

        var result = await roleManager.UpdateAsync(role);

        if (result.Succeeded && !string.IsNullOrEmpty(newDescription))
        {
            await roleManager.SetRoleNameAsync(role, newDescription);
        }

        return result.ToApplicationResult();
    }

    /// <summary>
    /// Deletes a role from application.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<Result> DeleteRoleAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        return role is null
            ? Result.Success()
            : (await roleManager.DeleteAsync(role)).ToApplicationResult();
    }

    /// <summary>
    /// Assigns a list of roles to a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task AssignRolesToUser(string userId, IEnumerable<string> roles)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");

        await userManager.AddToRolesAsync(user, roles);
    }

    /// <summary>
    /// Updates user role.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newRoles"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateUserRoles(string userId, IEnumerable<string> newRoles)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException("User not found");

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRolesAsync(user, newRoles);
    }

    /// <summary>
    /// Updates the identity user asynchronously in database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newUsername"></param>
    /// <param name="newEmail"></param>
    /// <returns></returns>
    public async Task<Result> UpdateUserAsync(string userId, string newUsername, string newEmail)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result.Failure(new[] { "User not found" });
        }

        var updateResults = new List<IdentityResult>();

        if (!string.IsNullOrWhiteSpace(newUsername) && user.UserName != newUsername)
        {
            user.UserName = newUsername;
            updateResults.Add(await userManager.UpdateAsync(user));
        }

        if (!string.IsNullOrWhiteSpace(newEmail) && user.Email != newEmail)
        {
            user.Email = newEmail;
            updateResults.Add(await userManager.SetEmailAsync(user, newEmail));
        }

        return updateResults.All(r => r.Succeeded)
            ? Result.Success()
            : Result.Failure(updateResults.SelectMany(r => r.Errors).Select(e => e.Description));
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Enumerable.Empty<string>();
        }

        return await userManager.GetRolesAsync(user);
    }
}
