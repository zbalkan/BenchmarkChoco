using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace BenchmarkChoco
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 1, targetCount: 20)]
    public class ChocoStrategy
    {
        [Benchmark]
        public void ParsingExecutableOutputStrategy()
        {
            var choco = new ChocolateyFactory();
            choco.GetUninstallerEntries(r => Console.WriteLine(r.Message));
        }

        [Benchmark]
        public void UsingLibraryStrategy()
        {
            var choco = new ChocolateyFactoryWithLib();
            choco.GetUninstallerEntries(r => Console.WriteLine(r.Message));
        }
    }
}
