﻿namespace AuthApp.Domain.Common;

public interface IHasDomainEvent
{
    List<DomainEvent> DomainEvents { get; set; }
}

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        DateOccurred = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;

    public bool IsPublished { get; set; }
}
