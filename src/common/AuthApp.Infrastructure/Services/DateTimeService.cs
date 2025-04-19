using AuthApp.Application.Common.Interfaces;

namespace AuthApp.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
