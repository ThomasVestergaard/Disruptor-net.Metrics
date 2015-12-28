using System;
using System.Collections.Generic;
using Disruptor_net.Metrics.QueueItem;
using Disruptor_net.Metrics.Sinks;

namespace Disruptor_net.Metrics.EventHandlers
{
    public class FixedSecondCountReporter<T> : BaseMetricsEventHandler<T> where T : class, IRingBufferItem
    {
        private int SecondInterval { get; set; }
        private DateTimeOffset IntervalFirstDate { get; set; }

        public FixedSecondCountReporter(ISink metricSink, int secondInterval)
            : base(metricSink)
        {
            SecondInterval = secondInterval;
            IntervalFirstDate = DateTimeOffset.MinValue;
        }

        public FixedSecondCountReporter(IEnumerable<ISink> metricSinks, int secondInterval)
            : base(metricSinks)
        {
            SecondInterval = secondInterval;
            IntervalFirstDate = DateTimeOffset.MinValue;
        }

        private void Flush()
        {
            FlushMetricsToSinks();
            IntervalFirstDate = DateTimeOffset.MinValue;
        }

        public override void OnNext(IRingBufferItem data, long sequence, bool endOfBatch)
        {
            if (IntervalFirstDate == DateTimeOffset.MinValue)
            {
                IntervalFirstDate = data.FirstTouchTime;
            }
            else
            {
                if ((data.FirstTouchTime - IntervalFirstDate).TotalSeconds >= SecondInterval)
                    Flush();
            }

            Aggregator.HandleItem(data);
        }
    }
}
