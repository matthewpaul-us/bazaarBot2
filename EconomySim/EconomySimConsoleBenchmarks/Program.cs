using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DoranAndParberryEconomySim;
using EconomySim;
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
            //EconomyTests tests = new EconomyTests();
            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

            DoranAndParberryEconomy galacticEconomy = new DoranAndParberryEconomy(10);
            Market galacticMarket = galacticEconomy.GetMarket("default");
            galacticMarket.Simulate(1000);
        }

        [HtmlExporter]
        public class EconomyTests
        {
            [Params(10, 100)]
            public int Iterations { get; set; }

            //[Benchmark]
            //public void IterationTest()
            //{
            //    for (int i = 0; i < Iterations; i++)
            //    {
            //        int j = i;
            //    }
            //}

            [Benchmark]
            public void EconomySimBenchmark()
            {
                //TODO: create "Iterations" number of agents and simulate
                //This is to simulate each planet in game being an agent...

                DoranAndParberryEconomy galacticEconomy = new DoranAndParberryEconomy(Iterations);
                Market galacticMarket = galacticEconomy.GetMarket("default");
                galacticMarket.Simulate(1);
            }
        }
    }
}
