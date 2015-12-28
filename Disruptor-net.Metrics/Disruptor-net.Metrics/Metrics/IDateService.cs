using System;

namespace Disruptor_net.Metrics.Metrics
{
    public interface IDateService
    {
        DateTimeOffset UtcNow();
    }
}
