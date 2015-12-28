using System;
using Disruptor_net.Metrics.QueueItem;

namespace Disruptor_net.Tests
{
    public class QItem : IRingBufferItem
    {
        public DateTimeOffset FirstTouchTime { get; set; }
    }
}
