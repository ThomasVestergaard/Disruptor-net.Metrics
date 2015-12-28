using Disruptor;
using Disruptor_net.Metrics.QueueItem;

namespace Disruptor_net.Metrics.EventHandlers
{
    public interface IMetricsEventHandler<T> : IEventHandler<IRingBufferItem> where T : class, IRingBufferItem
    {
        void Setup(RingBuffer<T> ringBuffer);
    }
}
