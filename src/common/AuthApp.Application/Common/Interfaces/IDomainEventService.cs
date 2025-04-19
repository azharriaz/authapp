using AuthApp.Domain.Common;

namespace AuthApp.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
