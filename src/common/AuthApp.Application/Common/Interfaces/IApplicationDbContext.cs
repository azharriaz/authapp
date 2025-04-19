using AuthApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Application.Common.Interfaces;

/// <summary>
/// application db context contract
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Product> Products { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
