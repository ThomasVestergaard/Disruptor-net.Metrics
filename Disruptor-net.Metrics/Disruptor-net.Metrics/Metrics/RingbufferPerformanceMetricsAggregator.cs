using System;
using Disruptor_net.Metrics.QueueItem;

namespace Disruptor_net.Metrics.Metrics
{
    public class RingbufferPerformanceMetricsAggregator : IRingbufferPerformanceMetricsAggregator
    {
        private readonly IDateService dateService;
        private double duration;
        private double firstToLastMilliseconds;
        private double messagesPerMilisecond;
        private bool FirstMessageTimeIsSet { get; set; }
        private DateTimeOffset FirstMessageTime { get; set; }
        private DateTimeOffset LastMessageTime { get; set; }

        private double minMeasuredDuration { get; set; }
        private double maxMeasuredDuration { get; set; }
        
        private double runningBaseAverage { get; set; }
        private long counter { get; set; }

        public RingbufferPerformanceMetricsAggregator(IDateService dateService)
        {
            this.dateService = dateService;
            FirstMessageTimeIsSet = false;

            Reset();
        }

        public void HandleItem(IRingBufferItem ringBufferItem)
        {
            counter++;
            duration = (dateService.UtcNow() - ringBufferItem.FirstTouchTime).TotalMilliseconds;

            if (!FirstMessageTimeIsSet)
            {
                FirstMessageTime = ringBufferItem.FirstTouchTime;
                FirstMessageTimeIsSet = true;
            }

            if (duration < minMeasuredDuration)
                minMeasuredDuration = duration;

            if (duration > maxMeasuredDuration)
                maxMeasuredDuration = duration;

            // Calculate running average (erighted average)
            runningBaseAverage = ((runningBaseAverage * (counter - 1)) + duration) / counter;
            LastMessageTime = ringBufferItem.FirstTouchTime;
        }

        public IRingbufferPerformanceMetrics Reset()
        {
            var toReturn = new RingbufferPerformanceMetrics();
            toReturn.ItemCount = counter;

            if (counter > 0)
            {
                toReturn.MinProcessTimeMilliseconds = minMeasuredDuration;
                toReturn.MaxProcessTimeMilliseconds = maxMeasuredDuration;
                toReturn.FirstMessageTime = FirstMessageTime;
                toReturn.LastMessageTime = LastMessageTime;
                toReturn.AverageTimeInQueueMs = runningBaseAverage;

                firstToLastMilliseconds = (LastMessageTime - FirstMessageTime).TotalMilliseconds;
                toReturn.MessagesPerSecond = (toReturn.ItemCount/firstToLastMilliseconds)*1000d;
            }

            FirstMessageTimeIsSet = false;

            minMeasuredDuration = Double.MinValue;
            maxMeasuredDuration = Double.MinValue;
            runningBaseAverage = Double.MinValue;
            counter = 0;
            return toReturn;
        }

       
    }
}