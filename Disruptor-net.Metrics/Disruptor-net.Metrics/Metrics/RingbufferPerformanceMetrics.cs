using System;

namespace Disruptor_net.Metrics.Metrics
{
    public class RingbufferPerformanceMetrics : IRingbufferPerformanceMetrics
    {
        public int RingBufferAvailablePercentage { get; set; }
        public double MinProcessTimeMilliseconds { get; set; }
        public double MaxProcessTimeMilliseconds { get; set; }
        public double AverageTimeInQueueMs { get; set; }
        public DateTimeOffset FirstMessageTime { get; set; }
        public DateTimeOffset LastMessageTime { get; set; }
        public long ItemCount { get; set; }
        public double MessagesPerSecond { get; set; }

        public RingbufferPerformanceMetrics()
        {
            MinProcessTimeMilliseconds = -1;
            MaxProcessTimeMilliseconds = -1;
            RingBufferAvailablePercentage = -1;
            MessagesPerSecond = -1;
            AverageTimeInQueueMs = -1;
            FirstMessageTime = DateTimeOffset.MinValue;
            LastMessageTime = DateTimeOffset.MinValue;
            ItemCount = 0;
        }

    }
}