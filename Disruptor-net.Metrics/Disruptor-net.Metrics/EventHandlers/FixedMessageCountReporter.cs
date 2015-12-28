using System.Collections.Generic;
using Disruptor_net.Metrics.QueueItem;
using Disruptor_net.Metrics.Sinks;

namespace Disruptor_net.Metrics.EventHandlers
{
    public class FixedMessageCountReporter<T> : BaseMetricsEventHandler<T> where T : class, IRingBufferItem
    {
        private int MessageCountInterval { get; set; }
        private int Counter { get; set; }

        public FixedMessageCountReporter(ISink metricSink, int messageCountInterval) : base(metricSink)
        {
            MessageCountInterval = messageCountInterval;
            Counter = 0;
        }

        public FixedMessageCountReporter(IEnumerable<ISink> metricSinks, int messageCountInterval) : base(metricSinks)
        {
            MessageCountInterval = messageCountInterval;
            Counter = 0;
        }

        public override void OnNext(IRingBufferItem data, long sequence, bool endOfBatch)
        {
            Counter++;
            Aggregator.HandleItem(data);

            if (Counter >= MessageCountInterval)
            {
                FlushMetricsToSinks();
                Counter = 0;
            }

        }
    }
}
