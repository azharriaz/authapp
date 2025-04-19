using AuthApp.Application.Common.Interfaces;
using AuthApp.Infrastructure.Identity;

using Duende.IdentityServer.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace AuthApp.Infrastructure.Persistence
{
    /// <summary>
    /// application db context
    /// </summary>
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IDomainEventService domainEventService) : base(options, operationalStoreOptions)
        {
            _dateTime = dateTime;
            _domainEventService = domainEventService;
            _currentUserService = currentUserService;
        }

        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// adds user details on record to be created/updated
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        //{
        //    foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.Entity.Creator = _currentUserService.UserId;
        //                entry.Entity.CreateDate = _dateTime.Now;
        //                break;
        //            case EntityState.Modified:
        //                entry.Entity.Modifier = _currentUserService.UserId;
        //                entry.Entity.ModifyDate = _dateTime.Now;
        //                break;
        //        }
        //    }

        //    var result = await base.SaveChangesAsync(cancellationToken);

        //    await DispatchEvents();

        //    return result;
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        ///// <summary>
        ///// dispatches event for subscribers to listen and action
        ///// </summary>
        ///// <returns></returns>
        //private async Task DispatchEvents()
        //{
        //    while (true)
        //    {
        //        var domainEventEntity = ChangeTracker
        //            .Entries<IHasDomainEvent>()
        //            .Select(x => x.Entity.DomainEvents)
        //            .SelectMany(x => x)
        //            .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

        //        if (domainEventEntity == null) break;

        //        domainEventEntity.IsPublished = true;
        //        await _domainEventService.Publish(domainEventEntity);
        //    }
        //}
    }
}
