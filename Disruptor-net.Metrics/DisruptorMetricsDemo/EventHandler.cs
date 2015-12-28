using System;
using System.Threading;
using Disruptor;

namespace DisruptorMetricsDemo
{
    public class EventHandler : IEventHandler<QueueItem>
    {
        private string Name { get; set; }
        private long lastSequenceNo;

        public EventHandler(string name)
        {
            Name = name;
            lastSequenceNo = -1;
        }

        public void OnNext(QueueItem data, long sequence, bool endOfBatch)
        {
            if (lastSequenceNo != -1)
            {
                if (sequence != (lastSequenceNo + 1))
                {
                    throw new InvalidOperationException("Message skipped!");
                }

            }
            
            //data.FirstTouchTime = DateTimeOffset.UtcNow;
            //Console.WriteLine("({4}) Received seqno: {0} on thread id {3}. Int: {1}, String: {2}", sequence, data.IntValue, data.StringValue, Thread.CurrentThread.ManagedThreadId, Name);
            lastSequenceNo = sequence;

            
        }
    }
}
