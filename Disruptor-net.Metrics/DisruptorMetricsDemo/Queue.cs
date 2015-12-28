using System;
using System.Threading.Tasks;
using Disruptor.Dsl;
using Disruptor;
using Disruptor_net.Metrics.EventHandlers;
using Disruptor_net.Metrics.Sinks;

namespace DisruptorMetricsDemo
{
    public class Queue
    {
        private int ringbufferSize = (int)Math.Pow(128, 2);

        private RingBuffer<QueueItem> ringBuffer;
        private Disruptor<QueueItem> disruptor;

        private IMetricsEventHandler<QueueItem> metricsEventHandler;

        public void Start()
        {
            var eventHandler = new EventHandler("A");


            metricsEventHandler = new FixedSecondCountReporter<QueueItem>(new ColoredConsoleSink(), 1);

            //disruptor = new Disruptor<QueueItem>(() => new QueueItem(), ringbufferSize, TaskScheduler.Default);
            
            disruptor = new Disruptor<QueueItem>(() => new QueueItem(),  new SingleThreadedClaimStrategy(ringbufferSize), new BusySpinWaitStrategy() , TaskScheduler.Default);
            disruptor.HandleExceptionsWith(new FatalExceptionHandler());
            disruptor.HandleEventsWith(metricsEventHandler);


            ringBuffer = disruptor.Start();
            metricsEventHandler.Setup(ringBuffer);

        }

        

        public void Stop()
        {
            disruptor.Halt();
        }

        public void Enqueue(QueueItem item)
        {
            item.FirstTouchTime = DateTimeOffset.UtcNow;
            var next = ringBuffer.Next();
            var entry = ringBuffer[next];
            entry.Update(item);
            ringBuffer.Publish(next);
            //Console.WriteLine("Enqueing on thread id {0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
