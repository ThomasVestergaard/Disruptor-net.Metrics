using Disruptor_net.Metrics.Metrics;

namespace Disruptor_net.Metrics.Sinks
{
    public interface ISink
    {
        void ReportMetrics(IRingbufferPerformanceMetrics metrics);
    }
}
