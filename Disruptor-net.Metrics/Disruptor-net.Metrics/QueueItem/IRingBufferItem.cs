using System;

namespace Disruptor_net.Metrics.QueueItem
{
    public interface IRingBufferItem
    {
        DateTimeOffset FirstTouchTime { get; set; }

    }
}
