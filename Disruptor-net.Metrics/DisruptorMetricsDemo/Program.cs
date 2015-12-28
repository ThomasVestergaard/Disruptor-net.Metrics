using System;
using System.Threading;
using System.Threading.Tasks;

namespace DisruptorMetricsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new Queue();
            queue.Start();

            Console.WriteLine("Hit any to start.");
            Console.ReadKey();

            string key = "";
            
            var qItem1 = new QueueItem { IntValue = 10, StringValue = "Blarh" };
            while (key != "a")
            {

                queue.Enqueue(qItem1);
                //Thread.Sleep(1);
                //Console.ReadKey();
            }
        

            queue.Stop();


        }
    }
}
