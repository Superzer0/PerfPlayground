using BenchmarkDotNet.Running;
using PerfPlayground.Benchmark;

namespace PerfPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var bench = new BenchmarkAccessToVariable();
            bench.RunLazy();*/
            //BenchmarkRunner.Run(typeof(Program).Assembly);

            var threadPlayground = new ThreadLongRunning();
            threadPlayground.Run();

        }
    }
}
