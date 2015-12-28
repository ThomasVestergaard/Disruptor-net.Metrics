using System;

namespace Disruptor_net.Metrics.Metrics
{
    public class DateService : IDateService
    {
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}