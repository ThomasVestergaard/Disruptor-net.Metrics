using System;

namespace Disruptor_net.Metrics.Metrics
{
    public interface IRingbufferPerformanceMetrics
    {
        long ItemCount { get; set; }
        int RingBufferAvailablePercentage { get; set; }
        double MinProcessTimeMilliseconds { get; set; }
        double MaxProcessTimeMilliseconds { get; set; }
        double AverageTimeInQueueMs { get; set; }
        DateTimeOffset FirstMessageTime { get; set; }
        DateTimeOffset LastMessageTime { get; set; }
        double MessagesPerSecond { get; set; }
    }
}
