using Disruptor_net.Metrics.QueueItem;

namespace Disruptor_net.Metrics.Metrics
{
    public interface IRingbufferPerformanceMetricsAggregator
    {
        void HandleItem(IRingBufferItem ringBufferItem);
        IRingbufferPerformanceMetrics Reset();
    }
}
