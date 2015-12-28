using System;
using Disruptor_net.Metrics.Metrics;
using NUnit.Framework;
using Rhino.Mocks;

namespace Disruptor_net.Tests
{
    [TestFixture]
    public class RingbufferPerformanceMetricsAggregatorTests
    {
        [Test]
        public void ShouldReturnMetricsWithDefaultValuesWhenNoMessagesHasBeenHandled()
        {
            var aggregator = new RingbufferPerformanceMetricsAggregator(new DateService());

            var metrics = aggregator.Reset();
            Assert.AreEqual(0, metrics.ItemCount);
            Assert.AreEqual(-1, metrics.MaxProcessTimeMilliseconds);
            Assert.AreEqual(-1, metrics.MinProcessTimeMilliseconds);
            Assert.AreEqual(-1, metrics.AverageTimeInQueueMs);
            Assert.AreEqual(DateTimeOffset.MinValue, metrics.FirstMessageTime);
            Assert.AreEqual(DateTimeOffset.MinValue, metrics.LastMessageTime);
        }

        [Test]
        public void ShouldReturnMetrics()
        {
            var dateService = new DateService();
            var aggregator = new RingbufferPerformanceMetricsAggregator(dateService);

            var d1 = dateService.UtcNow().AddSeconds(-10);
            var d2 = dateService.UtcNow().AddSeconds(-5);
            var d3 = dateService.UtcNow().AddSeconds(-3);
            var d4 = dateService.UtcNow().AddSeconds(-1);
            
            var qItem1 = new QItem() {FirstTouchTime = d1};
            var qItem2 = new QItem() {FirstTouchTime = d2};
            var qItem3 = new QItem() {FirstTouchTime = d3};
            var qItem4 = new QItem() {FirstTouchTime = d4};

            aggregator.HandleItem(qItem1);
            aggregator.HandleItem(qItem2);
            aggregator.HandleItem(qItem3);
            aggregator.HandleItem(qItem4);

            var metrics = aggregator.Reset();
            Assert.AreEqual(4, metrics.ItemCount);
            Assert.AreEqual(d1, metrics.FirstMessageTime);
            Assert.AreEqual(d4, metrics.LastMessageTime);
            Assert.IsTrue(metrics.AverageTimeInQueueMs > 0);
            Assert.IsTrue(metrics.MinProcessTimeMilliseconds > 0);
            Assert.IsTrue(metrics.MaxProcessTimeMilliseconds> 0);
        }

        [Test]
        public void ShouldResetToDefaultValues()
        {
            var dateService = new DateService();
            var aggregator = new RingbufferPerformanceMetricsAggregator(dateService);

            var d1 = dateService.UtcNow().AddSeconds(-10);
            var d2 = dateService.UtcNow().AddSeconds(-5);
            var d3 = dateService.UtcNow().AddSeconds(-3);
            var d4 = dateService.UtcNow().AddSeconds(-1);

            var qItem1 = new QItem() { FirstTouchTime = d1 };
            var qItem2 = new QItem() { FirstTouchTime = d2 };
            var qItem3 = new QItem() { FirstTouchTime = d3 };
            var qItem4 = new QItem() { FirstTouchTime = d4 };

            aggregator.HandleItem(qItem1);
            aggregator.HandleItem(qItem2);
            aggregator.HandleItem(qItem3);
            aggregator.HandleItem(qItem4);

            var metrics = aggregator.Reset();
            Assert.AreEqual(4, metrics.ItemCount);
            Assert.AreEqual(d1, metrics.FirstMessageTime);
            Assert.AreEqual(d4, metrics.LastMessageTime);
            Assert.IsTrue(metrics.AverageTimeInQueueMs > 0);
            Assert.IsTrue(metrics.MinProcessTimeMilliseconds > 0);
            Assert.IsTrue(metrics.MaxProcessTimeMilliseconds > 0);


            metrics = aggregator.Reset();
            Assert.AreEqual(0, metrics.ItemCount);
            Assert.AreEqual(-1, metrics.MaxProcessTimeMilliseconds);
            Assert.AreEqual(-1, metrics.MinProcessTimeMilliseconds);
            Assert.AreEqual(-1, metrics.AverageTimeInQueueMs);
            Assert.AreEqual(DateTimeOffset.MinValue, metrics.FirstMessageTime);
            Assert.AreEqual(DateTimeOffset.MinValue, metrics.LastMessageTime);

        }

    }
}
