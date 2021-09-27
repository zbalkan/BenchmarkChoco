using BenchmarkDotNet.Running;
using System;

namespace BenchmarkChoco
{
    public static class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<ChocoStrategy>();
            Console.WriteLine(summary.TotalTime.ToString());
        }
    }
}
