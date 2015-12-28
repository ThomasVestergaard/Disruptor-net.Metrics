using System;
using Disruptor_net.Metrics.Metrics;

namespace Disruptor_net.Metrics.Sinks
{
    public class ColoredConsoleSink : ISink
    {
        public void ReportMetrics(IRingbufferPerformanceMetrics metrics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Available ringbuffer: {0} %", metrics.RingBufferAvailablePercentage);
            Console.WriteLine("Items processed: {0}", metrics.ItemCount);
            Console.WriteLine("Average time in queue (ms): {0}", metrics.AverageTimeInQueueMs);
            Console.WriteLine("Messages per sec.: {0}", metrics.MessagesPerSecond.ToString("N0"));

        }
    }
}
