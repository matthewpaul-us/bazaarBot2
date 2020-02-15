using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomySimConsoleBenchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            IterationTests tests = new IterationTests();
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }

        [HtmlExporter]
        public class IterationTests
        {
            [Params(10, 100, 1000, 10000, 100000, 1000000, 1000000000)]
            public int Iterations { get; set; }

            [Benchmark]
            public void IterationTest()
            {
                for (int i = 0; i < Iterations; i++)
                {
                    int j = i;
                }
            }
        }
    }
}
