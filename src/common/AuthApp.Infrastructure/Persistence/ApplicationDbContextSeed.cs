using AuthApp.Domain.Entities;
using AuthApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthApp.Infrastructure.Persistence;

/// <summary>
/// seeds application data to database
/// </summary>
public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var defaultUser = new ApplicationUser 
        {
            UserName = "azharriaz", 
            Email = "azharwaraich268@gmail.com",
            Gsm = string.Empty,
            Name = "Azhar",
            LastName = "Riaz"
        };

        if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
        {
            await userManager.CreateAsync(defaultUser, "Azhar@321");
            await userManager.AddToRolesAsync(defaultUser, new[] { administratorRole.Name });
        }
    }

    public static async Task SeedApplicationDataAsync(ApplicationDbContext context)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(new Product[]
            {
                new Product()
                {
                    Name = "Angular Speedster Board 2000",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Green Angular Board 3000",
                    Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Core Board Speed Rush 3",
                    Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Net Core Super Board",
                    Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Angular Speedster Board 2000",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "React Board Super Whizzy Fast",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Typescript Entry Board",
                    Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
                    Price = 29.0m
                },
                new Product()
                {
                    Name = "Core Blue Hat",
                    Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 29.0m
                }
            });

            await context.SaveChangesAsync();
        }
    }
}
