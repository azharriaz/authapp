﻿namespace AuthApp.Application.Dto;

/// <summary>
/// respresents class to hold user details
/// </summary>
public class ApplicationUserDto
{
    public ApplicationUserDto()
    {
    }
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public List<string> Roles { get; set; }
}
