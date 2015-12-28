using System.Collections.Generic;
using Disruptor;
using Disruptor_net.Metrics.Metrics;
using Disruptor_net.Metrics.QueueItem;
using Disruptor_net.Metrics.Sinks;

namespace Disruptor_net.Metrics.EventHandlers
{
    public abstract class BaseMetricsEventHandler<T> : IMetricsEventHandler<T> where T : class, IRingBufferItem
    {
        protected RingBuffer<T> RingBuffer { get; set; }
        protected IRingbufferPerformanceMetricsAggregator Aggregator { get; set; }

        private List<ISink> Sinks { get; set; }

        protected BaseMetricsEventHandler(IEnumerable<ISink> metricSinks)
        {
            Sinks = new List<ISink>();
            Sinks.AddRange(metricSinks);
            Aggregator = new RingbufferPerformanceMetricsAggregator(new DateService());
            Aggregator.Reset();
        }

        protected BaseMetricsEventHandler(ISink metricSink)
        {
            Sinks = new List<ISink>();
            Sinks.Add(metricSink);
            Aggregator = new RingbufferPerformanceMetricsAggregator(new DateService());
            Aggregator.Reset();
        }

        public abstract void OnNext(IRingBufferItem data, long sequence, bool endOfBatch);

        public void Setup(RingBuffer<T> ringBuffer)
        {
            RingBuffer = ringBuffer;
        }

        private int GetRingbufferAvailablePercentage()
        {
            var tenPercent = RingBuffer.BufferSize / 10;

            if (!RingBuffer.HasAvailableCapacity(0))
                return 0;

            for (int i = 1; i < 10; i++)
            {
                if (!RingBuffer.HasAvailableCapacity(tenPercent * i))
                    return 10 * i;
            }

            return 100;
        }

        protected void FlushMetricsToSinks()
        {
            var metrics = Aggregator.Reset();
            metrics.RingBufferAvailablePercentage = GetRingbufferAvailablePercentage();

            if (Sinks.Count == 1)
            {
                Sinks[0].ReportMetrics(metrics);
                return;
            }

            for (int i = 0; i < Sinks.Count; i++)
                Sinks[i].ReportMetrics(metrics);
            
        }
        
    }
}
