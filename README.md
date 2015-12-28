# Disruptor-net.Metrics
Easy performance montoring of your disruptor ringbuffers.

It will:
 - Tell you how many items has been processed.
 - Min, max and average time (milliseconds) each item spent in the queue.
 - How much (percentage) of your ringbuffer is avilable.
 - Dump this data in your favorite logging framework via sinks (yes I stole the sink concept from serilog).

It will have a small impact on the overall performance of your ringbuffer. You need to choose carefully how you use it - If you log too much too often you will ruin performance. Play around with the settings to find your sweet spot.


# Usage - 3 easy steps

1.
What ever you put in your ringbuffer, make it implement IRingBufferItem.

```
public class QueueItem : IRingBufferItem
    {
        public DateTimeOffset FirstTouchTime { get; set; }
        
        // All your other stuff here
        
        public void Update(QueueItem other)
        {
          ...
        }
    }
```

2.
When you enqueue a new item, set FirstTouchTime to UtcNow

```
  public void Enqueue(QueueItem item)
        {
            var next = ringBuffer.Next();
            var entry = ringBuffer[next];
            entry.Update(item);
            entry.FirstTouchTime = DateTimeOffset.UtcNow;
            ringBuffer.Publish(next);
        }
```

3.
Add the metrics eventhandler to your disruptor.

```
    var metricsEventHandler = new FixedSecondCountReporter<QueueItem>(new ColoredConsoleSink(), 1);    
    disruptor = Disruptor<QueueItem>(() => new QueueItem(), ringbufferSize, TaskScheduler.Default);

    disruptor.HandleEventsWith(eventHandler1, eventHandler2).Then(metricsEventHandler);
    
    ringBuffer = disruptor.Start();
    metricsEventHandler.Setup(ringBuffer); // Important: Do this AFTER disruptor.Start() has been run.
```
