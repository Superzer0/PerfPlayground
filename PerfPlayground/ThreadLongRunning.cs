using System;
using System.Threading;
using System.Threading.Tasks;

namespace PerfPlayground
{
    class ThreadLongRunning
    {
        public void Run()
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Main thread {Thread.CurrentThread.ManagedThreadId}");
                AsyncOne().Wait();

                Console.WriteLine("Press any key to end...");
                Console.ReadKey();
            });
        }

        static async Task AsyncThree()
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"AsyncThree Task.Run thread id:{Thread.CurrentThread.ManagedThreadId.ToString()}");
            });

            Console.WriteLine($"AsyncThree continuation thread id:{Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        static async Task AsyncTwo()
        {
            await AsyncThree();

            Console.WriteLine($"AsyncTwo continuation thread id:{Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        static async Task AsyncOne()
        {
            await AsyncTwo();

            Console.WriteLine($"AsyncOne continuation thread id:{Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

    }
}
