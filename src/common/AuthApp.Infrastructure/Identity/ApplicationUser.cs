﻿using Microsoft.AspNetCore.Identity;

namespace AuthApp.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }

    public string LastName { get; set; }

    public string Gsm { get; set; }
}
