using System;
using BenchmarkDotNet.Attributes;

namespace BenchmarkChoco
{
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

        }
    }
}
