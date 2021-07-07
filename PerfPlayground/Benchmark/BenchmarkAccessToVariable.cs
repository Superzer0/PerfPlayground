using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace PerfPlayground.Benchmark
{
    public class BenchmarkAccessToVariable
    {
        private const int ThreadsN = 50;
        private const int AccessAttempts = 1_0000000;

        private static void RunBenchmark(IValueFactory valueFactory)
        {
            var barrier = new Barrier(ThreadsN);
            var counter = 0;
            var taskList = new List<Task>();

            for (int i = 0; i < ThreadsN; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    //Interlocked.Increment(ref counter);
                    //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is reporting for duty and waits. I am {counter}");
                    barrier.SignalAndWait();
                    //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Fire!");
                    
                    DoWork(valueFactory);

                }, TaskCreationOptions.LongRunning);

                taskList.Add(task);
            }
            Task.WaitAll(taskList.ToArray());
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static void DoWork(IValueFactory valueFactory)
        {
            for (var i = 0; i < AccessAttempts; i++)
            {
                var value = valueFactory.GetValue();
            }
        }

        [Benchmark]
        public void RunLazy() => RunBenchmark(new LazyLockingFactory());

        [Benchmark]
        public void RunDoubleChecked() => RunBenchmark(new DoubleCheckedLockingFactory());

        [Benchmark]
        public void RunNaive() => RunBenchmark(new NaiveLockingFactory());
    }
}
