using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Application.Common.Mapping;

public static class MappingExtensions
{
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, TypeAdapterConfig configuration, CancellationToken cancellationToken)
        => queryable.ProjectToType<TDestination>(configuration).ToListAsync(cancellationToken);
}
